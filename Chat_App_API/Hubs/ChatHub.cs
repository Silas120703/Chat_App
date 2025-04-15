using Chat_App_Core.Services;
using Chat_App_Database.Entities;
using Chat_App_Database.Repositories;
using Chat_App_Shared.DTOs.MassageDTO;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_App_API.Hubs
{
	public class ChatHub : Hub
	{
		private readonly UserConnectionRepository _userConnectionRepository;
		private readonly GroupRepository _groupRepository;
		private readonly GroupMemberRepository _groupMemberRepository;

		public ChatHub( UserConnectionRepository userConnectionRepository, GroupRepository groupRepository,GroupMemberRepository groupMemberRepository)
		{
			_userConnectionRepository = userConnectionRepository;
			_groupRepository = groupRepository;
			_groupMemberRepository = groupMemberRepository;
		}

		
	}
}
