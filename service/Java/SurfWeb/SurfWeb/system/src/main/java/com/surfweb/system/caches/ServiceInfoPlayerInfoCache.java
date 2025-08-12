package com.surfweb.system.caches;

import lombok.Data;

/**
 * 服务信息中的玩家信息缓存
 */
@Data
public class ServiceInfoPlayerInfoCache {

    /**
     * 玩家id
     */
    private long playerId;

    private String name;

    /**
     * 在线时长(秒)
     */
    private float duration;

}
