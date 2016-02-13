# ClilocToTsvConverter

Program to convert a UO Cliloc file to Tab Separated Values (tsv) file, back and forth. By default it'll use Cliloc.enu, but you can easily change the extension on top of the scripts.

Note: This is not an addon for RunUO. It's a standalone program.

## Usage

Get the file `Cliloc.enu` from yout UO install dir, and copy it to this folder.  
Then, run `ClilocToTsv.bat`to generate the file `Cliloc.tsv`. 

## Editing Cliloc.tsv

To edit the tsv file, you can use LibreOffice to open it and set to import as follows:

- Separated by: Tab
- Text Qualifier: leave the field blank (do not let single/double quotes because it causes errors)

Or if you prefer, you can translate directly into a text editor, but remember that there should be a tab between columns (enable the visualization of tabs in your text editor, to make it easy).

## Building Cliloc.enu

Once you've done the changes, you can run `TsvToCliloc.bat` to compile the .tsv into a cliloc file.  
For this you need to have .Net Framework 4.0 or Mono installed on your computer.

## What do those .bat do?

They compile the scripts into an executables. Run the executables and then delete them. Just that. You can open those .bat using any text editor to see how simple it is.