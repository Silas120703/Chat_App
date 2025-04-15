using Chat_App_Core.Mappers;
using Chat_App_Database.Entities;
using Chat_App_Database.Repositories;
using Chat_App_Shared.DTOs.UserDTO;
using Chat_App_Shared.Helpers;
using Chat_App_Shared.Services;
using Microsoft.AspNetCore.Identity;
using static System.Net.WebRequestMethods;
using System.Data;
using System.Numerics;
using System.Threading.Tasks;

namespace Chat_App_Core.Services
{
	public class UserService : ServiceBase<User>
	{	
		private readonly UserRepository _userRepository;
		private readonly UserTempRepository _userTempRepository;
		private readonly EmailHelper _emailHelper;
		public UserService(UserRepository userRepository,UserTempRepository userTempRepository,EmailHelper emailHelper) : base(userRepository)
		{
			_userRepository = userRepository;
			_userTempRepository = userTempRepository;
			_emailHelper = emailHelper;
		}

		public async Task<User> GetByEmailAsync(string mail)
		{
			return await _userRepository.GetByEmailAsync(mail);
		}

		public  User GetByEmail(string mail)
		{
			return  _userRepository.GetByEmail(mail);
		}

		public async Task<User> GetByPhone(string phone)
		{
			return await _userRepository.GetByPhone(phone);
		}

		public async Task<UserTemp> Register(UserRegisterDTO userDto)
		{
			var userExist = _userTempRepository.GetByEmail(userDto.Email);
			if (userExist != null)
			{
				userExist.FirstName = userDto.FirstName;
				userExist.LastName = userDto.LastName;
				userExist.BirthDay = userDto.BirthDay;
				userExist.Gender = userDto.Gender;
				userExist.Email = userDto.Email;
				userExist.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
				userExist.Phone = userDto.Phone;
				userExist.Role = "User";
				userExist.OTP = new Random().Next(1000, 9999);
				userExist.OTPTime = DateTime.Now.AddMinutes(3);
				_userTempRepository.Update(userExist);
				await _emailHelper.SendEmailAsync(userExist.Email, "OTP của bạn là: ", userExist.OTP.ToString());
				return userExist;
			}
			var user = new UserTemp
			{
				FirstName = userDto.FirstName,
				LastName = userDto.LastName,
				BirthDay = userDto.BirthDay,
				Gender = userDto.Gender,
				Email = userDto.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
				Phone = userDto.Phone,
				Role = "User",
				OTP = new Random().Next(100000, 999999),
				OTPTime = DateTime.Now.AddMinutes(3)
			};
			_userTempRepository.Add(user);
			_emailHelper.SendEmailAsync(user.Email, "OTP của bạn là: ", user.OTP.ToString());
			return user;
		}

		public User Virify(string email,int OTP)
		{
			var user = _userTempRepository.GetByEmail(email);
			if (user == null)
			{
				return null;
			}
			if (user.OTP == OTP && user.OTPTime > DateTime.Now)
			{
				var newUser = new User
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					BirthDay = user.BirthDay,
					Gender = user.Gender,
					Email = user.Email,
					PasswordHash = user.PasswordHash,
					Phone = user.Phone,
					Role = user.Role,
					IsBlock = false,
				};
				_userRepository.Add(newUser);
				_userTempRepository.Delete(user);
				return newUser;
			}
			return null;

		}

		public User Login(UserLoginDTO userDto)
		{
			var user = _userRepository.GetByEmail(userDto.Email);
			if (user == null)
			{
				return null;
			}
			if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
			{
				return null;
			}
			return user;
		}
		public User Update(UserUpdateDTO userDto)
		{
			var user = _userRepository.GetByEmail(userDto.Email);
			user.FirstName = userDto.FirstName;
			user.LastName = userDto.LastName;
			user.BirthDay = userDto.BirthDay;
			user.Email = userDto.Email;
			user.ProfilePicture = userDto.ProfilePicture;
			user.Phone = userDto.Phone;
			user.Gender = userDto.Gender;
			_userRepository.Update(user);
			return user;
		}

		public User ChangePassWord(string email,string newPassword)
		{
			var user = _userRepository.GetByEmail(email);
			if (user == null)
			{
				return null;
			}
			else
			{
				user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
				_userRepository.Update(user);

			}
			return user;
		}

		public async Task<UserTemp> ForgotPassword(string email)
		{
			var user = _userRepository.GetByEmail(email);
			if (user == null)
			{
				return null;
			}
			var userTemp = _userTempRepository.GetByEmail(email);
			if (userTemp != null)
			{
				_userTempRepository.Delete(userTemp);
			}
			var userTemp1 = new UserTemp
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				BirthDay = user.BirthDay,
				Gender = user.Gender,
				Email = user.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash),
				Phone = user.Phone,
				Role = user.Role,
				OTP = new Random().Next(100000, 999999),
				OTPTime = DateTime.Now.AddMinutes(3)
			};
			_userTempRepository.Add(userTemp1);
			_emailHelper.SendEmailAsync(userTemp1.Email, "Bạn đang yêu cầu đổi mật khẩu OTP của bạn là: ", userTemp1.OTP.ToString());
			return userTemp1;
		}
		public async Task<UserTemp> VerifyForgotPassword(string email, int OTP)
		{
			var userTemp = _userTempRepository.GetByEmail(email);
			if (userTemp == null)
			{
				return null;
			}
			if (userTemp.OTP == OTP && userTemp.OTPTime > DateTime.Now)
			{
				return userTemp;
			}
			return null;
		}
		public User ResetPassword(string email, int OTP, string newPassword)
		{
			var user = _userRepository.GetByEmail(email);
			var userTemp = _userTempRepository.GetByEmail(email);
			if ( user == null)
			{
				return null;
			}
			if (userTemp == null)
			{
				return null;
			}
			if (userTemp.OTP == OTP )
			{
				user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
				_userRepository.Update(user);
				_userTempRepository.Delete(userTemp);
				return user;
			}
			return null;
		}
		public async Task<User> CheckBlock(string email)
		{
			var user = await _userRepository.GetCheckBlockByEmail(email);
			if (user == null)
			{
				return null;
			}
			return user;
		}
		public async Task<User> BlockUser(string email)
		{
			var user = await _userRepository.GetByEmailAsync(email);
			if (user == null)
			{
				return null;
			}
			user.IsBlock = true;
			_userRepository.Update(user);
			return user;
		}
		public async Task<User> UnBlockUser(string email)
		{
			var user = await _userRepository.GetByEmailAsync(email);
			if (user == null)
			{
				return null;
			}
			user.IsBlock = false;
			_userRepository.Update(user);
			return user;
		}

	}
}
