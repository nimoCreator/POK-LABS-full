import torch
import torch.nn as nn
import torch.nn.functional as F

# Moduł Multi-Head Self-Attention
class MultiHeadSelfAttention(nn.Module):
    def __init__(self, d_model, num_heads):
        super(MultiHeadSelfAttention, self).__init__()
        self.num_heads = num_heads
        self.d_model = d_model
        assert d_model % num_heads == 0
        self.depth = d_model // num_heads

        self.W_q = nn.Linear(d_model, d_model)
        self.W_k = nn.Linear(d_model, d_model)
        self.W_v = nn.Linear(d_model, d_model)

    def split_heads(self, x, batch_size):
        x = x.view(batch_size, -1, self.num_heads, self.depth)
        return x.permute(0, 2, 1, 3)

    def forward(self, query, key, value, mask):
        batch_size = query.size(0)
        query = self.W_q(query)
        key = self.W_k(key)
        value = self.W_v(value)

        query = self.split_heads(query, batch_size)
        key = self.split_heads(key, batch_size)
        value = self.split_heads(value, batch_size)

        attention_scores = torch.matmul(query, key.permute(0, 1, 3, 2)) / self.depth**0.5

        if mask is not None:
            attention_scores = attention_scores.masked_fill(mask == 0, -1e9)

        attention_weights = F.softmax(attention_scores, dim=-1)
        output = torch.matmul(attention_weights, value)
        output = output.permute(0, 2, 1, 3).contiguous().view(batch_size, -1, self.num_heads * self.depth)

        return output

# Moduł Feed-Forward
class FeedForward(nn.Module):
    def __init__(self, d_model, d_ff):
        super(FeedForward, self).__init__()
        self.linear1 = nn.Linear(d_model, d_ff)
        self.linear2 = nn.Linear(d_ff, d_model)

    def forward(self, x):
        x = F.relu(self.linear1(x))
        x = self.linear2(x)
        return x

# Moduł warstw kodera i dekodera (bez pozostałych szczegółów)
class EncoderLayer(nn.Module):
    def __init__(self, d_model, num_heads, d_ff):
        super(EncoderLayer, self).__init__()
        self.multihead_self_attention = MultiHeadSelfAttention(d_model, num_heads)
        self.feed_forward = FeedForward(d_model, d_ff)

    def forward(self, x, mask):
        attention_output = self.multihead_self_attention(x, x, x, mask)
        x = x + attention_output
        x = x + self.feed_forward(x)
        return x

class DecoderLayer(nn.Module):
    def __init__(self, d_model, num_heads, d_ff):
        super(DecoderLayer, self).__init__()
        self.masked_self_attention = MultiHeadSelfAttention(d_model, num_heads)
        self.cross_attention = MultiHeadSelfAttention(d_model, num_heads)
        self.feed_forward = FeedForward(d_model, d_ff)

    def forward(self, x, encoder_output, src_mask, tgt_mask):
        masked_attention_output = self.masked_self_attention(x, x, x, tgt_mask)
        x = x + masked_attention_output
        cross_attention_output = self.cross_attention(x, encoder_output, encoder_output, src_mask)
        x = x + cross_attention_output
        x = x + self.feed_forward(x)
        return x

# Warstwa Positional Embedding
class PositionalEmbedding(nn.Module):
    def __init__(self, max_seq_len, d_model):
        super(PositionalEmbedding, self).__init__()
        self.positional_embedding = nn.Embedding(max_seq_len, d_model)

    def forward(self, x):
        position = torch.arange(0, x.size(1)).unsqueeze(0).to(x.device)
        return self.positional_embedding(position)

# Pełny model Transformer GPT
class GPT(nn.Module):
    def __init__(self, vocab_size, d_model, num_heads, d_ff, num_layers, max_seq_len):
        super(GPT, self).__init__()
        self.embedding = nn.Embedding(vocab_size, d_model)
        self.positional_embedding = PositionalEmbedding(max_seq_len, d_model)
        self.encoder = nn.ModuleList([EncoderLayer(d_model, num_heads, d_ff) for _ in range(num_layers)])

    def forward(self, x, mask):
        x = self.embedding(x)
        x = x + self.positional_embedding(x)
        for layer in self.encoder:
            x = layer(x, mask)
        return x

# Przykład użycia modelu
vocab_size = 10000  # Przykładowy rozmiar słownika
d_model = 512
num_heads = 8
d_ff = 2048
num_layers = 6
max_seq_len = 512  # Maksymalna długość sekwencji
model = GPT(vocab_size, d_model, num_heads, d_ff, num_layers, max_seq_len)

# Przygotowanie maski zakazu look-ahead (dla maskowanego self-attention)
src_mask = torch.zeros(1, max_seq_len, max_seq_len)
src_mask = src_mask.bool()

# Przykładowe dane wejściowe
input_data = torch.randint(0, vocab_size, (1, max_seq_len))

# Wywołanie modelu
output = model(input_data, src_mask)
print(output)
