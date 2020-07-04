!Please notice this software is licensed under AGPL - GNU AFFERO GENERAL PUBLIC LICENSE

To use this project clone it to your local workspace, open it and run it.

Info:

Folder "DOSARE" is a folder containing 4 sample pdf's and 4 sample xml'sample

When running the program a job starts that searches recursively through the "DOSARE" folder and returns a list with all pdf files found. Afterwards it loops through all of them in async parralel threads and disables their print button whilst gathering their corresponding xml info and injecting it as metadata in the pdf file.

After each program execution you will receive a message dialog with a summary of what happened. 
Results are also logged in a log file at a path of your choice.

You can configure the "DOSARE" folder name and log path by opening the Config.xml file and configuring the tags found there.

Contact:

If you require assistance with this library you can contact me at alexandru.dorin.chelaru@gmail.com