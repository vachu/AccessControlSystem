using System;
using System.Collections.Generic;

/*
 * In the real production version of this system, this DAL shall be the gateway to all
 * DB and LDAP operations.  Now, for the demo / POC, this is just a mock for the DB and LDAP
 * 
 * All the test data for the demo are hard-coded here
 * */

namespace Crossover
{
	public static class AccessPointDAL
	{
		private struct ManagerRecord {
			internal string EmpID;
			internal string Dept;
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
			string empID;
			public EventRecord(DateTime time, string evID, string accPtId, string empId)
			{
				Time = time;
				EventID = evID;
				AccessPointID = accPtId;
				empID = empId;
			}
		}

		private static List<ManagerRecord> ManagerTable = new List<ManagerRecord>();
		private static List<EmployeeRecord> employeeTable = new List<EmployeeRecord>();
		private static List<EventRecord> EventTransactionTable = new List<EventRecord>();

		static AccessPointDAL ()
		{
			ManagerTable.Add (new ManagerRecord("emp1", "dept1"));
			ManagerTable.Add (new ManagerRecord("emp2", "dept2"));

			employeeTable.Add(new EmployeeRecord("emp1", "/site1/dept1/bldg1"));
			employeeTable.Add(new EmployeeRecord("emp2", "/site1/dept2/bldg1"));
			employeeTable.Add(new EmployeeRecord("emp3", "/site1/dept1/bldg1"));
			employeeTable.Add(new EmployeeRecord("emp4", "/site1/dept1/bldg1"));
			employeeTable.Add(new EmployeeRecord("emp5", "/site1/dept1/bldg1"));
			employeeTable.Add(new EmployeeRecord("emp6", "/site1/dept2/bldg1"));
			employeeTable.Add(new EmployeeRecord("emp7", "/site1/dept2/bldg1"));
			employeeTable.Add(new EmployeeRecord("emp8", "/site1/dept2/bldg1"));
		}

		public static void AddLoginEvent(string AccessPointID, string empID)
		{
			EventTransactionTable.Add(
				new EventRecord(
					DateTime.Now,
					"LOGIN",
					AccessPointID,
					empID
				)
			);
		}

		public static void AddLogoutEvent(string AccessPointID, string empID)
		{
			EventTransactionTable.Add(
				new EventRecord(
					DateTime.Now,
					"LOGOUT",
					AccessPointID,
					empID
				)
			);
		}

		public static void AddCheckinEvent(string AccessPointID, string empID)
		{
			EventTransactionTable.Add(
				new EventRecord(
					DateTime.Now,
					"IN",
					AccessPointID,
					empID
				)
			);
		}

		public static void AddCheckoutEvent(string AccessPointID, string empID)
		{
			EventTransactionTable.Add(
				new EventRecord(
					DateTime.Now,
					"OUT",
					AccessPointID,
					empID
				)
			);
		}

		public static bool Login(string mgrId, string AccessPointID)
		{
			foreach (var rec in ManagerTable) {
				if (rec.EmpID == mgrId) {
					AddLoginEvent (AccessPointID, mgrId);
					return true;
				}
			}
			return false;
		}

		public static bool Logout(string mgrId, string AccessPointID)
		{
			foreach (var rec in ManagerTable) {
				if (rec.EmpID == mgrId) {
					AddLogoutEvent (AccessPointID, mgrId);
					return true;
				}
			}
			return false;
		}
	}
}

