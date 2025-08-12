package com.surfweb.system.dto.newrecords;

import lombok.Data;

@Data
public class NewRecordDto_Player {

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
     * 日期
     */
    private java.util.Date date;

    private float gapTime;

}
