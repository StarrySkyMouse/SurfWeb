package com.surfweb.web.controller;

import com.surfweb.common.core.controller.BaseController;
import com.surfweb.system.dto.rankings.RankingDto;
import com.surfweb.common.enums.RankingTypeEnum;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import com.surfweb.system.service.IPlayerCompleteServices;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

/**
 * 排行
 *
 * @author ruoyi
 */
@RestController
@RequestMapping("/Ranking")
public class RankingController extends BaseController {

    @Autowired
    private  IPlayerCompleteServices playerCompleteServices;


    /**
     * 获取排名列表
     */
    @GetMapping("/getRankingList")
    public List<RankingDto> getRankingList(@RequestParam("rankingType") RankingTypeEnum rankingType) {
        return playerCompleteServices.getRankingList(rankingType);
    }
}
