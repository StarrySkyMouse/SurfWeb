package com.surfweb.system.service;

import com.surfweb.system.domain.PlayerModel;
import com.surfweb.system.dto.players.PlayerFailDto;
import com.surfweb.system.dto.players.PlayerInfoDto;
import com.surfweb.system.dto.players.PlayerSucceesDto;
import com.surfweb.common.enums.RecordTypeEnum;

import java.util.List;
import java.util.Map;

public interface IPlayerServices {

    /**
     * 获取玩家信息
     */
    PlayerInfoDto getPlayerInfo(long id);

    /**
     * 获取玩家已完成Count
     */
    int getPlayerSucceesCount(long id, RecordTypeEnum recordType, String difficulty);

    /**
     * 获取玩家已完成List
     */
    List<PlayerSucceesDto> getPlayerSucceesList(long id, RecordTypeEnum recordType, String difficulty, int pageIndex);

    /**
     * 获取玩家未完成Count
     */
    int getPlayerFailCount(long id, RecordTypeEnum recordType, String difficulty);

    /**
     * 获取玩家未完成List
     */
    List<PlayerFailDto> getPlayerFailList(long id, RecordTypeEnum recordType, String difficulty, int pageIndex);

    /**
     * 获取玩家列表分页数据
     */
    List<PlayerModel> getPlayerPageList(int pageIndex, int pageSize);

    /**
     * 通过Auth获取(玩家Id,玩家名称)列表
     */
    Map<Integer, PlayerIdNamePair> getPlayerInfoListByAuth(List<Integer> authList);

    /**
     * 更新玩家信息
     */
    void updateStatsNumber();

    /**
     * 修改信息
     */
    void changeInfo(List<PlayerModel> changeList);

    /**
     * 通过玩家名称获取玩家信息
     */
    List<PlayerModel> getPlayersByNames(List<String> names);

    // 辅助类型声明
    class PlayerIdNamePair {
        public long playerId;
        public String playerName;
        // 构造函数和getter/setter可自动生成
    }
}
