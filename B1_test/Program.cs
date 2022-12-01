using B1_test;
int countOfFiles = 100;
Task1 task1 = new Task1(countOfFiles);
task1.CreateFiles();
task1.JoinFilesAsync("kym");
task1.AddStringsInDb();
