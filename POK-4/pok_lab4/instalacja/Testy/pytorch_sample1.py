import torch
import torch.nn as nn

# Definicja klasy EncoderLayer
class EncoderLayer(nn.Module):
    def __init__(self, d_model, nhead, dim_feedforward, dropout=0.1):
        super(EncoderLayer, self).__init__()
        self.self_attn = nn.MultiheadAttention(d_model, nhead)
        self.feed_forward = nn.Sequential(
            nn.Linear(d_model, dim_feedforward),
            nn.ReLU(),
            nn.Linear(dim_feedforward, d_model)
        )
        self.norm1 = nn.LayerNorm(d_model)
        self.norm2 = nn.LayerNorm(d_model)
        self.dropout = nn.Dropout(dropout)

    def forward(self, src):
        src2 = self.self_attn(src, src, src)[0]
        src = src + self.dropout(src2)
        src2 = self.feed_forward(src)
        src = src + self.dropout(src2)
        return self.norm2(src)

# Definicja klasy Transformer
class Transformer(nn.Module):
    def __init__(self, d_model, nhead, num_encoder_layers, dim_feedforward, dropout=0.1):
        super(Transformer, self).__init__()
        self.encoder_layers = nn.ModuleList([EncoderLayer(d_model, nhead, dim_feedforward, dropout) for _ in range(num_encoder_layers)])

    def forward(self, src):
        for layer in self.encoder_layers:
            src = layer(src)
        return src

# Przykład użycia modelu
d_model = 512
nhead = 8
num_encoder_layers = 6
dim_feedforward = 2048
dropout = 0.1

model = Transformer(d_model, nhead, num_encoder_layers, dim_feedforward, dropout)

# Przykład przekazania danych do modelu
input_data = torch.rand(10, 32, d_model)  # Przykładowe dane wejściowe
output = model(input_data)  # Wywołanie modelu
print(output)
