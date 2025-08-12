package com.surfweb.system.caches;

import lombok.Data;

/**
 * 地图主线缓存
 */
@Data
public class MapMainCache {

    /**
     * 地图ID
     */
    private long id;

    /**
     * 地图名称
     */
    private String name;

    /**
     * 难度
     */
    private String difficulty;

    /**
     * 图片
     */
    private String img;

    /**
     * 地图奖励关数量
     */
    private int bonusNumber;

    /**
     * 地图阶段数量
     */
    private int stageNumber;
}
