using System;
using System.Collections.Generic;
using System.Collections;
using ServerProtocol.Dto;
using UnityEngine;

namespace MyModel
{

    public class RoomModel
    {
        public List<MatchRoomUserInfoDto> roomUserList { get; private set; }

        public RoomModel(MatchRoomDto roomDto)
        {
            roomUserList = roomDto.userList;
            ResetPosition();
        }

        /// <summary>
        /// 通过id获取房间中的用户
        /// </summary>
        private MatchRoomUserInfoDto getRoomUser(int userId)
        {
            int index = getRoomUserIndex(userId);
            if (index == -1)
                return null;
            else
                return roomUserList[index];
        }

        /// <summary>
        /// 通过id获取房间中的用户在列表中的下标
        /// </summary>
        private int getRoomUserIndex(int userId)
        {
            for (int i = 0; i < roomUserList.Count; i++)
            {
                if (roomUserList[i].userInfo.userId == userId)
                    return i;
            }
            return -1;
        }


        public void Ready(UserInfoDto userDto) // 服务器发来的广播的对象是UserInfoDto
        {
            getRoomUser(userDto.userId).ready = true;
        }

        public void UnReady(UserInfoDto userDto)
        {
            getRoomUser(userDto.userId).ready = false;
        }

        public void EnterRoom(UserInfoDto userDto) //fixme 顺序问题
        {
            MatchRoomUserInfoDto roomUser = new MatchRoomUserInfoDto() { ready = false, userInfo = userDto, placeIndex = roomUserList.Count };
            roomUserList.Add(roomUser);
            // 直接加入会打乱相对顺序，排序以保证相对顺序与服务器一致。
            roomUserList.Sort((a, b) => a.placeIndex.CompareTo(b.placeIndex));
            ResetPosition();
        }

        public void ExitRoom(UserInfoDto userDto)
        {
            roomUserList.Remove(getRoomUser(userDto.userId));
        }


        /// <summary>
        /// 旋转位次，使得客户端用户变成1号位，同时他们的顺序不变，即每一个人的前后是谁没有变。
        /// </summary>
        private void ResetPosition()
        {
            int selfUserId = Models.gameModel.userInfoDto.userId;
            int index = getRoomUserIndex(selfUserId);
            if (index == -1)
            {
                Debug.LogError("自己不在房间中！");
                return;
            }

            //把当前用户前面的放到列表末尾
            List<MatchRoomUserInfoDto> tempList = roomUserList.GetRange(0, index);
            roomUserList.RemoveRange(0, index);
            roomUserList.AddRange(tempList);
        }
    }
}
