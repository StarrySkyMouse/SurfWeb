package com.surfweb.system.service;

import com.surfweb.system.domain.PlayerCompleteModel;
import com.surfweb.system.dto.players.PlayertimesDataDto;
import com.surfweb.system.dto.rankings.RankingDto;
import com.surfweb.common.enums.RankingTypeEnum;
import com.surfweb.common.enums.RecordTypeEnum;
import javafx.util.Pair;
import java.util.Date;
import java.util.List;
import java.util.Map;

public interface IPlayerCompleteServices {

    /**
     * 获取最后一次的时间
     */
    Pair<Date, Date> getFinallyDateTime();

    /**
     * 通过时间查询
     */
    List<PlayerCompleteModel> getByDate(Date date, List<RecordTypeEnum> typeList);

    /**
     * 处理增量过来的数据还未完成关联的
     */
    void disposeDataAssociation();

    /**
     * 隐藏不喜欢的数据
     */
    void hideUnLikeData();

    /**
     * 获取旧的数据(主线和奖励)
     */
    List<PlayerCompleteModel> getOldPlayertimesData(List<PlayertimesDataDto> list);

    /**
     * 获取旧的数据(阶段)
     */
    List<PlayerCompleteModel> getOldStagetimesData(List<PlayertimesDataDto> list);

    /**
     * 获取排名
     */
    List<RankingDto> getRankingList(RankingTypeEnum rankingType);
}
