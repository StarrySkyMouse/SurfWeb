package com.surfweb.system.domain;

import com.surfweb.common.core.domain.BaseEntity;
import com.surfweb.common.enums.RecordTypeEnum;
import lombok.Data;

@Data
public class PlayerCompleteModel extends BaseEntity {
    /**
     * 腐竹玩家id
     */
    private int auth;

    /**
     * 玩家id
     */
    private long playerId;

    /**
     * 玩家名
     */
    private String playerName;

    /**
     * 地图id
     */
    private long mapId;

    /**
     * 地图名称
     */
    private String mapName;

    /**
     * 通关类型(0-主线，1-奖励，2-阶段)
     */
    private RecordTypeEnum type;

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

    /**
     * 是否隐藏
     */
    private boolean hide;
}
