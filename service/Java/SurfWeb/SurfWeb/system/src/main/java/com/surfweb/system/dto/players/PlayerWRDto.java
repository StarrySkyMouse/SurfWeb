package com.surfweb.system.dto.players;

import lombok.Data;

import java.util.List;

@Data
public class PlayerWRDto {

    /**
     * 地图Id
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

    private List<PlayerStageDto> stages;
}
