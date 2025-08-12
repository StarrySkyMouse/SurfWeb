package com.surfweb.system.caches;


import com.surfweb.common.enums.RecordTypeEnum;
import lombok.Data;

import java.util.Date;

/**
 * 地图WR缓存
 */
@Data
public class MapWrCache {

    /**
     * 地图id
     */
    private long mapId;

    /**
     * 地图名称
     */
    private String mapName;

    /**
     * 难度
     */
    private String difficulty;

    /**
     * 地图图片
     */
    private String img;

    /**
     * 玩家id
     */
    private long playerId;

    /**
     * 玩家名
     */
    private String playerName;
    /**
     * 时间
     */
    private float time;
    /**
     * 日期
     */
    private Date date;
    /**
     * 通关类型(0-主线，1-奖励，2-阶段)
     */
    private RecordTypeEnum type;
    /**
     * 阶段
     */
    private Integer stage;

}