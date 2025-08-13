package com.surfweb.system.service.impl;

import com.surfweb.common.utils.PageUtils;
import com.surfweb.system.domain.PlayerModel;
import com.surfweb.system.dto.players.PlayerFailDto;
import com.surfweb.system.dto.players.PlayerInfoDto;
import com.surfweb.system.dto.players.PlayerInfoListByAuthDto;
import com.surfweb.system.dto.players.PlayerSucceesDto;
import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.mapper.PlayerCompleteMapper;
import com.surfweb.system.mapper.PlayerMapper;
import lombok.var;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.surfweb.system.service.IPlayerServices;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

@Service
public class PlayerServices implements IPlayerServices {

    @Autowired
    private PlayerMapper playerMapper;
    @Autowired
    private MapServices mapServices;

    @Override
    public PlayerInfoDto getPlayerInfo(long id) {
        return playerMapper.getPlayerInfo(id);
    }

    @Override
    public int getPlayerSucceesCount(long id, RecordTypeEnum recordType, String difficulty) {
        return playerMapper.getPlayerSucceesCount(id, recordType, difficulty);
    }

    @Override
    public List<PlayerSucceesDto> getPlayerSucceesList(long id, RecordTypeEnum recordType, String difficulty, int pageIndex) {
        var result = playerMapper.getPlayerSucceesList(id, recordType, difficulty, pageIndex);
        //查询wr信息
        var mapWrList = mapServices.getMapWrList(recordType).stream()
                .filter(t -> result.stream().anyMatch(a -> a.getMapId() == t.getMapId()) && t.getType() == recordType)
                .collect(Collectors.toList());
        for (var item : result) {
            mapWrList.stream().filter(t -> t.getMapId() == item.getMapId()).findFirst().ifPresent(t -> {
                for (var stage : item.getStages()) {
                    stage.setGapTime(t.getTime() - stage.getTime());
                }
            });
        }
        return result;
    }

    @Override
    public int getPlayerFailCount(long id, RecordTypeEnum recordType, String difficulty) {
        return playerMapper.getPlayerFailCount(id, recordType, difficulty);
    }

    @Override
    public List<PlayerFailDto> getPlayerFailList(long id, RecordTypeEnum recordType, String difficulty, int pageIndex) {
        List<PlayerFailDto> result = new ArrayList<>();
        if (recordType == RecordTypeEnum.MAIN) {
            result = playerMapper.getPlayerFailList_Main(id, difficulty, pageIndex);
        } else if (recordType == RecordTypeEnum.BOUNTY) {
            result = playerMapper.getPlayerFailList_Bounty(id, difficulty, pageIndex);
        } else {
            result = playerMapper.getPlayerFailList_Stage(id, difficulty, pageIndex);
        }
        return result;
    }

    @Override
    public List<PlayerModel> getPlayerPageList(int pageIndex, int pageSize) {
        PageUtils.startPage(pageIndex,100);
        return playerMapper.getPlayerPageList();
    }
    @Override
    public List<PlayerInfoListByAuthDto> getPlayerInfoListByAuth(List<Integer> authList) {
        return playerMapper.getPlayerInfoListByAuth(authList);
    }
    @Override
    public void updateStatsNumber() {
        playerMapper.updateStatsNumber();
    }

    @Override
    public void changeInfo(List<PlayerModel> changeList) {
        playerMapper.changeInfo(changeList);
    }

    @Override
    public List<PlayerModel> getPlayersByNames(List<String> names) {
        return playerMapper.getPlayersByNames(names);
    }
}
