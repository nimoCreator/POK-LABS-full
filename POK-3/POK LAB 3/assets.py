from pathlib import Path
from time import perf_counter


def inputWithDefault(prompt, default, value_type=None):
	value_type = value_type or type(default)
	value = input("{0} [{1}]: ".format(prompt, default)).strip()

	if value == "":
		return default

	try:
		value = value_type(value)
	except ValueError:
		return default

	if isinstance(value, (int, float)) and value <= 0:
		return default

	return value


class DoubleWriter:
	def __init__(self, file_name):
		self.file_path = Path("./results/"+file_name+".txt")
		self.encoding = "utf-8"
		self.start_time = perf_counter()
		self.closed = False

		self.file_path.parent.mkdir(parents=True, exist_ok=True)
		self.file = open(self.file_path, "w", encoding=self.encoding)
		
		self.print("====================================")
		self.print("ZADANIE ", file_name)
		self.print("------------------------------------")

	def print(self, *objects, sep=" ", end="\n", flush=False):
		text = sep.join(str(obj) for obj in objects) + end
		self.file.write(text)
		if flush:
			self.file.flush()
		print(*objects, sep=sep, end=end, flush=flush)

	def close(self):
		if self.closed:
			return

		elapsed_time = perf_counter() - self.start_time
		self.print()
		self.print("------------------------------------")
		self.print("rozwiazanie zajelo: {0:.3f} ms".format(elapsed_time * 1000))
		self.print("====================================")
		self.file.close()
		self.closed = True

	def __del__(self):
		try:
			self.close()
		except Exception:
			pass
