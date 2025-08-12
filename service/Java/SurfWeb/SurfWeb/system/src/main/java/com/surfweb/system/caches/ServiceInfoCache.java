package com.surfweb.system.caches;

import lombok.Data;

import java.util.List;

/**
 * 服务信息缓存
 */
@Data
public class ServiceInfoCache {
    private String map;
    private ServiceInfoMapInfoCache mapInfo;
    private List<ServiceInfoPlayerInfoCache> playerInfos;
    private int maxPlayers;
}
