using System;
using System.IO;
using System.Text.Json;
using OfficeOpenXml;

namespace PersonalFinances;

class Program
{
	static void Main(string[] args)
	{
		try
		{
			bool quitApp = false;

			do
			{
				Console.WriteLine($"Plese select an option (add, quit):");
				var selectedOption = Console.ReadLine();

				switch (selectedOption)
				{
					case "add":
						Expense expense = GetNewExpense();

						if (expense != null)
							SaveExpense(expense);

						break;
					case "quit":
						quitApp = true;
						break;
				}
			}
			while (!quitApp);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	private static Expense GetNewExpense()
	{
		Console.WriteLine("Date (dd/mm/yyyy):");
		var userDateInput = Console.ReadLine();
		var date = new DateTime(Convert.ToInt16(userDateInput.Substring(6, 4)), Convert.ToInt16(userDateInput.Substring(3, 2)), Convert.ToInt16(userDateInput[..2]));

		Console.WriteLine("Payee:");
		var payee = Console.ReadLine();

		Categories category = GetCategory();

		PaymentMethods paymentMethod = GetPaymentMethod();

		Console.WriteLine("Amount:");
		var amount = Convert.ToDecimal(Console.ReadLine());

		Console.WriteLine("Detail");
		var detail = Console.ReadLine();

		var expense = new Expense(Guid.NewGuid(), paymentMethod, date, amount, payee, detail, category);

		Console.WriteLine(expense.ToString());

		return expense;
	}

	private static PaymentMethods GetPaymentMethod()
	{
		Console.WriteLine("Payment method:");
		for (int i = 0; i < Enum.GetNames(typeof(PaymentMethods)).Length; i++)
		{
			Console.WriteLine($"{i + 1}. {Enum.GetNames(typeof(PaymentMethods))[i]}");
		}

		bool validPaymentMethodInput = false;

		do
		{
			if (int.TryParse(Console.ReadLine(), out int userPaymentMethodInput)
				&& userPaymentMethodInput >= 1
				&& userPaymentMethodInput <= Enum.GetNames(typeof(PaymentMethods)).Length)
			{
				PaymentMethods paymentMethod = (PaymentMethods)(userPaymentMethodInput - 1);
				return paymentMethod;
			}
			else
			{
				Console.WriteLine("Invalid payment method.");
			}
		} while (!validPaymentMethodInput);

		throw new Exception("Error ocurred while getting the category from the user.");
	}

	private static Categories GetCategory()
	{
		Console.WriteLine("Category");
		for (int i = 0; i < Enum.GetNames(typeof(Categories)).Length; i++)
		{
			Console.WriteLine($"{i + 1}. {Enum.GetNames(typeof(Categories))[i]}");
		}

		bool validCategoryInput = false;

		do
		{
			if (int.TryParse(Console.ReadLine(), out int userCategoryInput)
				&& userCategoryInput >= 1
				&& userCategoryInput <= Enum.GetNames(typeof(Categories)).Length)
			{
				Categories category = (Categories)(userCategoryInput - 1);
				return category;
			}
			else
			{
				Console.WriteLine("Invalid category. Please enter a valid category:");
			}
		} while (!validCategoryInput);

		throw new Exception("Error ocurred while getting the category from the user.");
	}

	private static Result SaveExpense(Expense expense)
	{
		string filePath = @"/Users/enmanuelleacuna/Documents/Personal finances.xlsx";

		using var personalFinancesExcel = new ExcelPackage(new FileInfo(filePath));

		var recordsWorksheet = personalFinancesExcel.Workbook.Worksheets[0];

		if (recordsWorksheet != null)
		{
			// Find the last row with data in column A
			int lastRow = recordsWorksheet.Cells["A" + recordsWorksheet.Dimension.End.Row].End.Row;
			int newRow = lastRow + 1; // Add a new row

			// Set values in the new row
			recordsWorksheet.Cells["A" + newRow].Value = expense.Id.ToString();
			recordsWorksheet.Cells["B" + newRow].Value = expense.Date;
			recordsWorksheet.Cells["C" + newRow].Value = expense.Amount;
			recordsWorksheet.Cells["D" + newRow].Value = expense.PaymentMethod.ToString();
			recordsWorksheet.Cells["E" + newRow].Value = expense.Payee;
			recordsWorksheet.Cells["F" + newRow].Value = expense.Detail;
			recordsWorksheet.Cells["G" + newRow].Value = expense.Category;
		}
		else
		{
			Console.WriteLine($"Worksheet {recordsWorksheet} not found.");
		}

		personalFinancesExcel.Save();

		Console.WriteLine("Saved.");

		return Result.Success();
	}
}
