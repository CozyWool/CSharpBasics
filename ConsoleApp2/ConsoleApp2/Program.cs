using ConsoleApp2;

//DecodeLab("МУ_ЛР_1_2.pdf");
return;

void DecodeLab(string filePath, int start = 100000, int end = 999999)
{
    PasswordDecoder.FindDatabaseLabPassword(filePath, start, end);
}