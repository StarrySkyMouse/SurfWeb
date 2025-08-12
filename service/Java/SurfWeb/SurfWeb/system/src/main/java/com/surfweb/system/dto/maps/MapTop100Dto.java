package com.surfweb.system.dto.maps;

import lombok.Data;

import java.util.Date;

@Data
public class MapTop100Dto {

    /**
     * 排行
     */
    private int ranking;

    /**
     * 玩家id
     */
    private long playerId;

    /**
     * 玩家名
     */
    private String playerName;

    /**
     * 阶段
     */
    private Integer stage;

    /**
     * 通关时间
     */
    private float time;

    /**
     * 时间差距
     */
    private float gapTime;

    /**
     * 日期
     */
    private Date date;
}
