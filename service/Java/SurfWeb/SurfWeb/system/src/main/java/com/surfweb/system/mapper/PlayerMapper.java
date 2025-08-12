package com.surfweb.system.mapper;

import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.domain.PlayerModel;
import com.surfweb.system.dto.players.PlayerFailDto;
import com.surfweb.system.dto.players.PlayerInfoDto;
import com.surfweb.system.dto.players.PlayerSucceesDto;
import com.surfweb.system.service.IPlayerServices;
import org.apache.ibatis.annotations.Param;

import java.util.List;
import java.util.Map;

public interface PlayerMapper {
    PlayerInfoDto getPlayerInfo(@Param("id") long id);

    //todo:MyBatis待实现
    int getPlayerSucceesCount(
            @Param("id") long id,
            @Param("recordType") RecordTypeEnum recordType,
            @Param("difficulty") String difficulty
    );

    //todo:MyBatis待实现
    List<PlayerSucceesDto> getPlayerSucceesList(@Param("id") long id, @Param("recordType") RecordTypeEnum recordType, @Param("difficulty") String difficulty, @Param("pageIndex") int pageIndex);

    //todo:MyBatis待实现
    int getPlayerFailCount(
            @Param("id") long id,
            @Param("recordType") RecordTypeEnum recordType,
            @Param("difficulty") String difficulty
    );

    //todo:MyBatis待实现
    List<PlayerFailDto> getPlayerFailList(
            @Param("id") long id,
            @Param("recordType") RecordTypeEnum recordType,
            @Param("difficulty") String difficulty,
            @Param("pageIndex") int pageIndex
    );

    //todo:分页
    List<PlayerModel> getPlayerPageList(
            @Param("pageIndex") int pageIndex,
            @Param("pageSize") int pageSize
    );

    //todo:MyBatis待实现
    Map<Integer, IPlayerServices.PlayerIdNamePair> getPlayerInfoListByAuth(
            @Param("authList") List<Integer> authList
    );

    //todo:MyBatis待实现
    void updateStatsNumber();

    //todo:MyBatis待实现
    void changeInfo(
            @Param("changeList") List<PlayerModel> changeList
    );

    List<PlayerModel> getPlayersByNames(
            @Param("names") List<String> names
    );
}