using PIS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIS.User.Core.Domain
{
	public class UserLogin:EntityBase
	{
		public int UserId { get; set; } 
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }
	}
}
