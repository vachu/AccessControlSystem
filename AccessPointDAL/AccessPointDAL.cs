using System;
using System.Collections.Generic;

namespace Crossover
{
	public static class AccessPointDAL
	{
		private struct ManagerRecord {
			string EmpID;
			string Dept;
			public ManagerRecord(string id, string dept)
			{
				EmpID = id;
				Dept = dept;
			}
		}

		private struct EmployeeRecord {
			string EmpID;
			string AccessPointID;
			public EmployeeRecord(string id, string accPtId)
			{
				EmpID = id;
				AccessPointID = accPtId;
			}
		}

		private struct EventRecord {
			DateTime Time;
			string EventID;
			string AccessPointID;
			string EmpID;
			public EventRecord(DateTime time, string evID, string accPtId, string empId)
			{
				Time = time;
				EventID = evID;
				AccessPointID = accPtId;
				EmpID = empId;
			}
		}

		private static List<ManagerRecord> ManagerTable = new List<ManagerRecord>();
		private static List<EmployeeRecord> EmployeeTable = new List<EmployeeRecord>();
		private static List<EventRecord> EventTransactionTable = new List<EventRecord>();

		static AccessPointDAL ()
		{
			ManagerTable.Add (new ManagerRecord("Emp1", "Dept_1"));
			ManagerTable.Add (new ManagerRecord("Emp2", "Dept_2"));

			EmployeeTable.Add(new EmployeeRecord("Emp1", "/Site1/Dept_1/Bldg_1"));
			EmployeeTable.Add(new EmployeeRecord("Emp2", "/Site1/Dept_2/Bldg_1"));
			EmployeeTable.Add(new EmployeeRecord("Emp3", "/Site1/Dept_1/Bldg_1"));
			EmployeeTable.Add(new EmployeeRecord("Emp4", "/Site1/Dept_1/Bldg_1"));
			EmployeeTable.Add(new EmployeeRecord("Emp5", "/Site1/Dept_1/Bldg_1"));
			EmployeeTable.Add(new EmployeeRecord("Emp6", "/Site1/Dept_2/Bldg_1"));
			EmployeeTable.Add(new EmployeeRecord("Emp7", "/Site1/Dept_2/Bldg_1"));
			EmployeeTable.Add(new EmployeeRecord("Emp8", "/Site1/Dept_2/Bldg_1"));
		}

		public static void AddLoginEvent(string AccessPointID, string EmpID)
		{
			EventTransactionTable.Add(
				new EventRecord(
					DateTime.Now,
					"LOGIN",
					AccessPointID,
					EmpID
				)
			);
		}

		public static void AddCheckinEvent(string AccessPointID, string EmpID)
		{
			EventTransactionTable.Add(
				new EventRecord(
					DateTime.Now,
					"IN",
					AccessPointID,
					EmpID
				)
			);
		}

		public static void AddCheckoutEvent(string AccessPointID, string EmpID)
		{
			EventTransactionTable.Add(
				new EventRecord(
					DateTime.Now,
					"OUT",
					AccessPointID,
					EmpID
				)
			);
		}
	}
}

