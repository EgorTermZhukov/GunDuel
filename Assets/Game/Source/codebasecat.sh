#!/bin/bash

# Define the root directory to start searching from
ROOT_DIR="./" # Replace with your actual folder name

# Define the output file
OUTPUT_FILE="all_cs_code.txt"

# Clear the output file if it exists
> "$OUTPUT_FILE"

# Find all .cs files recursively and concatenate their content
find "$ROOT_DIR" -name "*.cs" -print0 | while IFS= read -r -d $'\0' file; do
    echo -e "\n--- Content of $file ---\n" >> "$OUTPUT_FILE"
    cat "$file" >> "$OUTPUT_FILE"
    echo -e "\n\n" >> "$OUTPUT_FILE"
done

echo "All .cs files concatenated into $OUTPUT_FILE"
