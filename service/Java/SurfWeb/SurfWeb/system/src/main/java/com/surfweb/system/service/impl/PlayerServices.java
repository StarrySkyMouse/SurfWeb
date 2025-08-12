package com.surfweb.system.service.impl;

import com.surfweb.system.domain.PlayerModel;
import com.surfweb.system.dto.players.PlayerFailDto;
import com.surfweb.system.dto.players.PlayerInfoDto;
import com.surfweb.system.dto.players.PlayerSucceesDto;
import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.mapper.PlayerCompleteMapper;
import com.surfweb.system.mapper.PlayerMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.surfweb.system.service.IPlayerServices;

import java.util.Collections;
import java.util.List;
import java.util.Map;

@Service
public class PlayerServices implements IPlayerServices {

    @Autowired
    private PlayerMapper playerMapper;

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
        return playerMapper.getPlayerSucceesList(id, recordType, difficulty, pageIndex);
    }

    @Override
    public int getPlayerFailCount(long id, RecordTypeEnum recordType, String difficulty) {
        return playerMapper.getPlayerFailCount(id, recordType, difficulty);
    }

    @Override
    public List<PlayerFailDto> getPlayerFailList(long id, RecordTypeEnum recordType, String difficulty, int pageIndex) {
        return playerMapper.getPlayerFailList(id, recordType, difficulty, pageIndex);
    }

    @Override
    public List<PlayerModel> getPlayerPageList(int pageIndex, int pageSize) {
        return playerMapper.getPlayerPageList(pageIndex, pageSize);
    }

    @Override
    public Map<Integer, PlayerIdNamePair> getPlayerInfoListByAuth(List<Integer> authList) {
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
