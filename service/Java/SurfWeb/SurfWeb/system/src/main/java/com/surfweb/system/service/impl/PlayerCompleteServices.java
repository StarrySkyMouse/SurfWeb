package com.surfweb.system.service.impl;

import com.surfweb.system.domain.PlayerCompleteModel;
import com.surfweb.system.dto.players.PlayertimesDataDto;
import com.surfweb.system.dto.rankings.RankingDto;
import com.surfweb.common.enums.RankingTypeEnum;
import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.mapper.PlayerCompleteMapper;
import javafx.util.Pair;
import lombok.var;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.surfweb.system.service.IPlayerCompleteServices;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.List;

@Service
public class PlayerCompleteServices implements IPlayerCompleteServices {

    @Autowired
    private PlayerCompleteMapper playerCompleteMapper;

    @Override
    public Pair<Date, Date> getFinallyDateTime() {
        return (Pair<Date, Date>) playerCompleteMapper.getFinallyDateTime();
    }

    @Override
    public List<PlayerCompleteModel> getByDate(Date date, List<RecordTypeEnum> typeList) {
        return playerCompleteMapper.getByDate(date, typeList);
    }

    @Override
    public void disposeDataAssociation() {
        playerCompleteMapper.disposeDataAssociation_Player();
        playerCompleteMapper.disposeDataAssociation_Map();
    }

    @Override
    public void hideUnLikeData() {
        playerCompleteMapper.hideUnLikeData_IsTrue();
        playerCompleteMapper.hideUnLikeData_IsFalse();
    }

    @Override
    public List<PlayerCompleteModel> getOldPlayertimesData(List<PlayertimesDataDto> list) {
        List<PlayerCompleteModel> result = new ArrayList<>();
        var batchSize = 100;
        var total = list.size();
        for (var i = 0; i < total; i += batchSize){
            int fromIndex = i;
            int toIndex = Math.min(i + batchSize, list.size());
            List<PlayertimesDataDto> batch = list.subList(fromIndex, toIndex);
            var subResult = playerCompleteMapper.getOldPlayertimesData(batch);
            result.addAll(subResult);
        }
        return result;
    }

    @Override
    public List<PlayerCompleteModel> getOldStagetimesData(List<PlayertimesDataDto> list) {
        return playerCompleteMapper.getOldStagetimesData(list);
    }

    @Override
    public List<RankingDto> getRankingList(RankingTypeEnum rankingType) {
        return playerCompleteMapper.getRankingList(rankingType);
    }
}
