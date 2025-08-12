package com.surfweb.system.dto.players;

import lombok.Data;

import java.math.BigDecimal;

@Data
public class PlayerInfoDto {

    private long id;

    /**
     * 玩家名
     */
    private String name;

    /**
     * 积分排行
     */
    private int integralRanking;

    /**
     * 积分
     */
    private BigDecimal integral;

    /**
     * 地图完成排行
     */
    private int succeesRanking;

    /**
     * 地图完成数
     */
    private int succeesNumber;

    /**
     * 主线wr排行
     */
    private int wrRanking;

    /**
     * 主线wr数
     */
    private int wrNumber;

    /**
     * 奖励wr排行
     */
    private int bwRanking;

    /**
     * 奖励wr数
     */
    private int bwrNumber;

    /**
     * 阶段wr排行
     */
    private int swRanking;

    /**
     * 阶段wr数
     */
    private int swrNumber;
}
