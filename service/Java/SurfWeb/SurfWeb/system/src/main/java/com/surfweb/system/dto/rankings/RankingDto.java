package com.surfweb.system.dto.rankings;

import com.surfweb.common.enums.RankingTypeEnum;
import lombok.Data;

import java.math.BigDecimal;

@Data
public class RankingDto {

    /**
     * 排名类型(积分)
     */
    private RankingTypeEnum type;

    /**
     * 玩家id
     */
    private long playerId;

    /**
     * 玩家名
     */
    private String playerName;

    /**
     * 数值
     */
    private BigDecimal value;

    /**
     * 进度
     */
    private String progress;
}
