package com.surfweb.system.domain;

import com.surfweb.common.core.domain.BaseEntity;
import com.surfweb.common.enums.RecordTypeEnum;
import lombok.Data;

@Data
public class PlayerModel extends BaseEntity {

    /**
     * 腐竹玩家id
     */
    private int auth;

    /**
     * 玩家名
     */
    private String name;

    /**
     * 积分
     */
    private java.math.BigDecimal integral;

    /**
     * 地图完成数
     */
    private int succeesNumber;

    /**
     * 主线wr数
     */
    private int wrNumber;

    /**
     * 奖励wr数
     */
    private int bwrNumber;

    /**
     * 阶段wr数
     */
    private int swrNumber;
}
