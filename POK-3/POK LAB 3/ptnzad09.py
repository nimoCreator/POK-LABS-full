from math import sqrt


class Vector:
    def __init__(self, values):
        if len(values) != 3:
            raise ValueError("Vector musi miec 3 elementy")
        self.values = [float(value) for value in values]

    def __len__(self):
        return 3

    def __getitem__(self, index):
        return self.values[index]

    def __add__(self, other):
        return Vector([self[i] + other[i] for i in range(3)])

    def __mul__(self, other):
        if isinstance(other, (int, float)):
            return Vector([other * self[i] for i in range(3)])
        return sum(self[i] * other[i] for i in range(3))

    __rmul__ = __mul__

    def norm(self):
        return sqrt(self * self)

    def __str__(self):
        return "[{0:8.3f}, {1:8.3f}, {2:8.3f}]".format(*self.values)


class Matrix:
    def __init__(self, rows):
        if len(rows) != 3 or any(len(row) != 3 for row in rows):
            raise ValueError("Matrix musi miec rozmiar 3 x 3")
        self.rows = [[float(value) for value in row] for row in rows]

    def __len__(self):
        return 3

    def __getitem__(self, index):
        return self.rows[index]

    def __add__(self, other):
        return Matrix([[self[i][j] + other[i][j] for j in range(3)] for i in range(3)])

    def __mul__(self, other):
        if isinstance(other, (int, float)):
            return Matrix([[other * self[i][j] for j in range(3)] for i in range(3)])
        if isinstance(other, Vector):
            return Vector([sum(self[i][j] * other[j] for j in range(3)) for i in range(3)])
        raise TypeError("Matrix mozna mnozyc przez liczbe albo Vector")

    __rmul__ = __mul__

    def trace(self):
        return sum(self[i][i] for i in range(3))

    def determinant(self):
        a = self.rows
        return (
            a[0][0] * a[1][1] * a[2][2]
            + a[0][1] * a[1][2] * a[2][0]
            + a[0][2] * a[1][0] * a[2][1]
            - a[0][2] * a[1][1] * a[2][0]
            - a[0][1] * a[1][0] * a[2][2]
            - a[0][0] * a[1][2] * a[2][1]
        )

    def __str__(self):
        return "\n".join("[{0:8.3f}, {1:8.3f}, {2:8.3f}]".format(*row) for row in self.rows)


def main():
    u = Vector([1, 2, 3])
    v = Vector([4, -2, 1])
    a = Matrix([[1, 2, 3], [0, -1, 4], [2, 1, 0]])
    b = Matrix([[2, 0, 1], [1, 3, 0], [-1, 2, 2]])

    print("u =", u)
    print("v =", v)
    print("u + v =", u + v)
    print("2 * u =", 2 * u)
    print("u * v =", u * v)
    print("|u| =", "{0:.6f}".format(u.norm()))
    print()
    print("A =")
    print(a)
    print()
    print("B =")
    print(b)
    print()
    print("A + B =")
    print(a + b)
    print()
    print("3 * A =")
    print(3 * a)
    print()
    print("A * u =", a * u)
    print("tr(A) =", a.trace())
    print("det(A) =", a.determinant())


if __name__ == "__main__":
    main()
