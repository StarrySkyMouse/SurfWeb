package com.surfweb.system.dto.players;

import lombok.Data;

import java.util.List;

@Data
public class PlayerFailDto {

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
     * 图片
     */
    private String img;

    /**
     * 阶段
     */
    private List<Integer> stages;
}
