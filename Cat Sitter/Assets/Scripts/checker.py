import os


def count_lines_in_cs_files(directory):
    total_lines = 0
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith(".cs"):
                try:
                    with open(
                        os.path.join(root, file), "r", encoding="utf-8"
                    ) as cs_file:
                        total_lines += len(cs_file.readlines())
                except Exception as e:
                    print(f"Could not read {file}: {e}")
    return total_lines


if __name__ == "__main__":
    current_directory = os.getcwd()
    total_lines = count_lines_in_cs_files(current_directory)
    print(f"Total lines in .cs files: {total_lines}")
