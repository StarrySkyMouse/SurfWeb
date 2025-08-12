package com.surfweb.system.caches;

import lombok.Data;
/**
 * 服务信息中的地图信息缓存
 */
@Data
public class ServiceInfoMapInfoCache {
    /**
     * 地图ID
     */
    private long id;

    /**
     * 难度
     */
    private String difficulty;

    /**
     * 地图图片
     */
    private String img;
}
