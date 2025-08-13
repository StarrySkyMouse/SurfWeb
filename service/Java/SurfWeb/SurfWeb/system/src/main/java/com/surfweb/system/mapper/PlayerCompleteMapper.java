package com.surfweb.system.mapper;

import com.surfweb.common.enums.RankingTypeEnum;
import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.domain.PlayerCompleteModel;
import com.surfweb.system.dto.players.PlayertimesDataDto;
import com.surfweb.system.dto.rankings.RankingDto;
import com.surfweb.system.service.IPlayerCompleteServices;
import javafx.util.Pair;
import org.apache.ibatis.annotations.Param;

import java.util.Date;
import java.util.List;
import java.util.Map;

public interface PlayerCompleteMapper {
    List<Date> getFinallyDateTime();

    List<PlayerCompleteModel> getByDate(@Param("date") Date date, @Param("typeList") List<RecordTypeEnum> typeList);

    void disposeDataAssociation_Player();
    void disposeDataAssociation_Map();

    void hideUnLikeData_IsTrue();
    void hideUnLikeData_IsFalse();

    List<PlayerCompleteModel> getOldPlayertimesData(@Param("list") List<PlayertimesDataDto> list);

    List<PlayerCompleteModel> getOldStagetimesData(@Param("list") List<PlayertimesDataDto> list);

    List<RankingDto> getRankingList_Integral();
    List<RankingDto> getRankingList_Type(@Param("rankingType") RankingTypeEnum rankingType);
}
