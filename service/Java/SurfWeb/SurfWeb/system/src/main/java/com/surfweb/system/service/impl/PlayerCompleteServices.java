package com.surfweb.system.service.impl;

import com.surfweb.system.caches.MapWrCache;
import com.surfweb.system.domain.PlayerCompleteModel;
import com.surfweb.system.dto.players.PlayertimesDataDto;
import com.surfweb.system.dto.rankings.RankingDto;
import com.surfweb.common.enums.RankingTypeEnum;
import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.mapper.PlayerCompleteMapper;
import javafx.scene.Group;
import javafx.util.Pair;
import lombok.var;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.surfweb.system.service.IPlayerCompleteServices;

import java.math.BigDecimal;
import java.util.*;
import java.util.stream.Collectors;

@Service
public class PlayerCompleteServices implements IPlayerCompleteServices {

    @Autowired
    private PlayerCompleteMapper playerCompleteMapper;
    @Autowired
    private MapServices mapServices;

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
        for (var i = 0; i < total; i += batchSize) {
            int fromIndex = i;
            int toIndex = Math.min(i + batchSize, list.size());
            var batch = list.subList(fromIndex, toIndex);
            var subResult = playerCompleteMapper.getOldPlayertimesData(batch);
            result.addAll(subResult);
        }
        return result;
    }

    @Override
    public List<PlayerCompleteModel> getOldStagetimesData(List<PlayertimesDataDto> list) {
        List<PlayerCompleteModel> result = new ArrayList<>();
        var batchSize = 100;
        var total = list.size();
        for (var i = 0; i < total; i += batchSize) {
            int fromIndex = i;
            int toIndex = Math.min(i + batchSize, list.size());
            var batch = list.subList(fromIndex, toIndex);
            var subResult = playerCompleteMapper.getOldStagetimesData(batch);
            result.addAll(subResult);
        }
        return result;
    }

    @Override
    public List<RankingDto> getRankingList(RankingTypeEnum rankingType) {
        List<RankingDto> result = new ArrayList<>();
        if (rankingType == RankingTypeEnum.INTEGRAL) {
            result = playerCompleteMapper.getRankingList_Integral();
        } else {
            var wrList = mapServices.getMapWrList(RecordTypeEnum.values()[rankingType.ordinal() - 1]);
            result = wrList.stream()
                    .collect(Collectors.groupingBy(MapWrCache::getPlayerId))
                    .values().stream()
                    .map(t -> {
                        var first = t.get(0);
                        RankingDto dto = new RankingDto();
                        dto.setType(rankingType);
                        dto.setPlayerId(first.getPlayerId());
                        dto.setPlayerName(first.getPlayerName());
                        dto.setValue(BigDecimal.valueOf(t.size()));
                        return dto;
                    })
                    //比较器，降序
                    .sorted((a, b) -> b.getValue().compareTo(a.getValue()))
                    .limit(10)
                    .collect(Collectors.toList());
        }
        return result;
    }
}
