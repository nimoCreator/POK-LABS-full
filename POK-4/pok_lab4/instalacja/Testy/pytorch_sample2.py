import torch
import torch.nn as nn

# Warstwa Multi-Head Self-Attention
class MultiHeadAttention(nn.Module):
    def __init__(self, d_model, nhead):
        super(MultiHeadAttention, self).__init__()
        self.attn = nn.MultiheadAttention(d_model, nhead)

    def forward(self, query, key, value, mask=None):
        return self.attn(query, key, value, attn_mask=mask)

# Warstwa Feed-Forward
class FeedForward(nn.Module):
    def __init__(self, d_model, dim_feedforward):
        super(FeedForward, self).__init__()
        self.linear1 = nn.Linear(d_model, dim_feedforward)
        self.relu = nn.ReLU()
        self.linear2 = nn.Linear(dim_feedforward, d_model)

    def forward(self, x):
        return self.linear2(self.relu(self.linear1(x)))

# Warstwa Encoder Layer
class EncoderLayer(nn.Module):
    def __init__(self, d_model, nhead, dim_feedforward, dropout=0.1):
        super(EncoderLayer, self).__init__()
        self.self_attn = MultiHeadAttention(d_model, nhead)
        self.feed_forward = FeedForward(d_model, dim_feedforward)
        self.norm1 = nn.LayerNorm(d_model)
        self.norm2 = nn.LayerNorm(d_model)
        self.dropout = nn.Dropout(dropout)

    def forward(self, src, mask=None):
        attn_output, _ = self.self_attn(src, src, src, mask=mask)
        src = src + self.dropout(attn_output)
        src = self.norm1(src)
        ff_output = self.feed_forward(src)
        src = src + self.dropout(ff_output)
        src = self.norm2(src)
        return src

# Warstwa Decoder Layer
class DecoderLayer(nn.Module):
    def __init__(self, d_model, nhead, dim_feedforward, dropout=0.1):
        super(DecoderLayer, self).__init__()
        self.self_attn = MultiHeadAttention(d_model, nhead)
        self.cross_attn = MultiHeadAttention(d_model, nhead)
        self.feed_forward = FeedForward(d_model, dim_feedforward)
        self.norm1 = nn.LayerNorm(d_model)
        self.norm2 = nn.LayerNorm(d_model)
        self.norm3 = nn.LayerNorm(d_model)
        self.dropout = nn.Dropout(dropout)

    def forward(self, tgt, memory, tgt_mask=None, memory_mask=None):
        tgt_attn_output, _ = self.self_attn(tgt, tgt, tgt, mask=tgt_mask)
        tgt = tgt + self.dropout(tgt_attn_output)
        tgt = self.norm1(tgt)
        cross_attn_output, _ = self.cross_attn(tgt, memory, memory, mask=memory_mask)
        tgt = tgt + self.dropout(cross_attn_output)
        tgt = self.norm2(tgt)
        ff_output = self.feed_forward(tgt)
        tgt = tgt + self.dropout(ff_output)
        tgt = self.norm3(tgt)
        return tgt

# Model Transformer z koderem i dekoderem
class Transformer(nn.Module):
    def __init__(self, num_encoder_layers, num_decoder_layers, d_model, nhead, dim_feedforward, src_vocab_size, tgt_vocab_size, max_seq_length, dropout=0.1):
        super(Transformer, self).__init__()
        self.embedding = nn.Embedding(src_vocab_size, d_model)
        self.encoder = nn.ModuleList([EncoderLayer(d_model, nhead, dim_feedforward, dropout) for _ in range(num_encoder_layers)])
        self.decoder = nn.ModuleList([DecoderLayer(d_model, nhead, dim_feedforward, dropout) for _ in range(num_decoder_layers)])
        self.fc = nn.Linear(d_model, tgt_vocab_size)
        self.max_seq_length = max_seq_length

    def forward(self, src, tgt):
        src = self.embedding(src)
        tgt = self.embedding(tgt)

        src_mask = self.generate_square_subsequent_mask(src.shape[0])
        tgt_mask = self.generate_square_subsequent_mask(tgt.shape[0])
        memory_mask = src_mask

        for layer in self.encoder:
            src = layer(src, src_mask)
        for layer in self.decoder:
            tgt = layer(tgt, src, tgt_mask, memory_mask)

        output = self.fc(tgt)
        return output

    def generate_square_subsequent_mask(self, size):
        mask = (torch.triu(torch.ones(size, size)) == 1).transpose(0, 1)
        mask = mask.float().masked_fill(mask == 0, float('-inf')).masked_fill(mask == 1, float(0.0))
        return mask

# Przykład użycia modelu
src_vocab_size = 10000
tgt_vocab_size = 10000
d_model = 512
nhead = 8
num_encoder_layers = 6
num_decoder_layers = 6
dim_feedforward = 2048
max_seq_length = 100
dropout = 0.1

model = Transformer(num_encoder_layers, num_decoder_layers, d_model, nhead, dim_feedforward, src_vocab_size, tgt_vocab_size, max_seq_length, dropout)

# Przykład przekazania danych do modelu
src_data = torch.randint(0, src_vocab_size, (max_seq_length,))
tgt_data = torch.randint(0, tgt_vocab_size, (max_seq_length,))
output = model(src_data, tgt_data)  # Wywołanie modelu
print(output)
