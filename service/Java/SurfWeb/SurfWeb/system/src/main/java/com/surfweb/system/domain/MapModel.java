package com.surfweb.system.domain;

import com.surfweb.common.core.domain.BaseEntity;
import lombok.Data;

@Data
public class MapModel extends BaseEntity {
    /**
     * 名称
     */
    private String name;
    /**
     * 难度
     */
    private String difficulty;
    /**
     * 地图图片
     */
    private String img;
    /**
     * 完成人数
     */
    private int surcessNumber;
    /**
     * 地图奖励关数量
     */
    private int bonusNumber;
    /**
     * 地图阶段数量
     */
    private int stageNumber;
}
