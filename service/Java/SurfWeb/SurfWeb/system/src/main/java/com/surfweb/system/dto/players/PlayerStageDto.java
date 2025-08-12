package com.surfweb.system.dto.players;

import lombok.Data;

import java.util.Date;

@Data
public class PlayerStageDto {

    /**
     * 阶段
     */
    private Integer stage;

    /**
     * 时间
     */
    private float time;

    /**
     * 和wr差距
     */
    private float gapTime;

    /**
     * 日期
     */
    private Date date;
}
