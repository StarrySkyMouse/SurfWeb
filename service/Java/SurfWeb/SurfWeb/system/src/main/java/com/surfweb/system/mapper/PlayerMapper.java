package com.surfweb.system.mapper;

import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.domain.PlayerModel;
import com.surfweb.system.dto.players.PlayerFailDto;
import com.surfweb.system.dto.players.PlayerInfoDto;
import com.surfweb.system.dto.players.PlayerInfoListByAuthDto;
import com.surfweb.system.dto.players.PlayerSucceesDto;
import com.surfweb.system.service.IPlayerServices;
import org.apache.ibatis.annotations.Param;

import java.util.List;
import java.util.Map;

public interface PlayerMapper {
    PlayerInfoDto getPlayerInfo(@Param("id") long id);

    int getPlayerSucceesCount(
            @Param("id") long id,
            @Param("recordType") RecordTypeEnum recordType,
            @Param("difficulty") String difficulty
    );

    List<PlayerSucceesDto> getPlayerSucceesList(@Param("id") long id, @Param("recordType") RecordTypeEnum recordType, @Param("difficulty") String difficulty, @Param("pageIndex") int pageIndex);

    int getPlayerFailCount(
            @Param("id") long id,
            @Param("recordType") RecordTypeEnum recordType,
            @Param("difficulty") String difficulty
    );

    List<PlayerFailDto> getPlayerFailList_Main(
            @Param("id") long id,
            @Param("difficulty") String difficulty,
            @Param("pageIndex") int pageIndex
    );

    List<PlayerFailDto> getPlayerFailList_Bounty(
            @Param("id") long id,
            @Param("difficulty") String difficulty,
            @Param("pageIndex") int pageIndex
    );

    List<PlayerFailDto> getPlayerFailList_Stage(
            @Param("id") long id,
            @Param("difficulty") String difficulty,
            @Param("pageIndex") int pageIndex
    );

    List<PlayerModel> getPlayerPageList();

    List<PlayerInfoListByAuthDto> getPlayerInfoListByAuth(
            @Param("authList") List<Integer> authList
    );

    void updateStatsNumber();

    void changeInfo(
            @Param("changeList") List<PlayerModel> changeList
    );

    List<PlayerModel> getPlayersByNames(
            @Param("names") List<String> names
    );
}