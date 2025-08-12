package com.surfweb.system.caches;

import lombok.Data;
/**
 * 地图奖励关或阶段缓存
 */
@Data
public class MapBountyOrStageCache {

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
     * 阶段
     */
    private int stage;
}
