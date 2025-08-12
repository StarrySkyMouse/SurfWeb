package com.surfweb.system.service.impl;

import com.surfweb.common.utils.PageUtils;
import com.surfweb.system.caches.MapBountyOrStageCache;
import com.surfweb.system.caches.MapMainCache;
import com.surfweb.system.caches.MapWrCache;
import com.surfweb.system.domain.MapModel;
import com.surfweb.system.dto.maps.MapDto;
import com.surfweb.system.dto.maps.MapListDto;
import com.surfweb.system.dto.maps.MapTop100Dto;
import com.surfweb.system.dto.newrecords.NewMapDto;
import com.surfweb.system.dto.newrecords.NewRecordDto;
import com.surfweb.system.dto.newrecords.NewRecordDto_Player;
import com.surfweb.system.dto.newrecords.PopularMapDto;
import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.mapper.MapMapper;
import lombok.var;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.surfweb.system.service.IMapServices;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

@Service
public class MapServices implements IMapServices {

    @Autowired
    private MapMapper mapMapper;

    @Override
    public MapDto getMapInfoById(long id) {
        return mapMapper.getMapInfoById(id);
    }

    @Override
    public MapModel getMapInfoByName(String name) {
        return mapMapper.getMapInfoByName(name);
    }

    @Override
    public List<MapModel> getMapInfoByNameList(List<String> names) {
        return mapMapper.getMapInfoByNameList(names);
    }

    @Override
    public int getMapCount(String difficulty, String search) {
        return mapMapper.getMapCount(difficulty, search);
    }

    @Override
    public List<MapListDto> getMapList(String difficulty, String search, int pageIndex) {
        return mapMapper.getMapList(difficulty, search, pageIndex);
    }

    @Override
    public int getMapTop100Count(long id, RecordTypeEnum recordType, Integer stage) {
        return mapMapper.getMapTop100Count(id, recordType, stage);
    }

    @Override
    public List<MapTop100Dto> getMapTop100List(long id, RecordTypeEnum recordType, Integer stage, int pageIndex) {
        var mapWr =this.getMapWrList(recordType)
                .stream()
                .filter(t->t.getMapId() == id)
                .filter(t->!(t.getStage()!=null && t.getType()!=RecordTypeEnum.MAIN)|| t.getStage().equals(stage))
                .findFirst().orElse(null);
        var result =  mapMapper.getMapTop100List(id, recordType, stage);
        if(!result.isEmpty()){
            for (var i=0;i<result.size();i++){
                result.get(i).setRanking((pageIndex-1)*10+i+i);
                if(mapWr!=null){
                    result.get(i).setGapTime(mapWr.getTime()-result.get(i).getGapTime());
                }
            }
        }
        return result;
    }

    @Override
    public Map<String, Long> getMapIdListByName(List<String> mapNameList) {
        return mapMapper.getMapIdListByName(mapNameList);
    }

    @Override
    public void updateSucceesNumber() {
        mapMapper.updateSucceesNumber();
    }

    @Override
    public List<MapWrCache> getMapWrList(RecordTypeEnum recordType) {
        return mapMapper.getMapWrList(recordType);
    }

    @Override
    public List<MapMainCache> getMapMainList() {
        return mapMapper.getMapMainList();
    }

    @Override
    public List<MapBountyOrStageCache> getMapBountyList() {
        return this.getMapMainList().stream()
                .filter(t -> t.getBonusNumber() != 0)
                .flatMap(t -> IntStream.rangeClosed(1, t.getBonusNumber())
                        .mapToObj(a -> {
                            var cache = new MapBountyOrStageCache();
                            cache.setId(t.getId());
                            cache.setName(t.getName());
                            cache.setDifficulty(t.getDifficulty());
                            cache.setImg(t.getImg());
                            cache.setStage(a);
                            return cache;
                        })).collect(Collectors.toList());
    }

    @Override
    public List<MapBountyOrStageCache> getMapStageList() {
        return this.getMapMainList().stream()
                .filter(t->t.getStageNumber()!=0)
                .flatMap(t->IntStream.rangeClosed(1,t.getStageNumber())
                        .mapToObj(a->{
                            var cache=new MapBountyOrStageCache();
                            cache.setId(t.getId());
                            cache.setName(t.getName());
                            cache.setDifficulty(t.getDifficulty());
                            cache.setImg(t.getImg());
                            cache.setStage(a);
                            return cache;
                        })).collect(Collectors.toList());
    }

    @Override
    public List<NewRecordDto> getNewRecordList(RecordTypeEnum recordType) {
        List<NewRecordDto> result = new ArrayList<>();
        var i = 1;
        var isQuit = false;
        while (true){
            PageUtils.startPage(i,100);
            var list = mapMapper.getNewRecordList(recordType);
            i++;
            if(list.isEmpty()) break;
            for (var t:list){
                var item =result.stream().filter(a->a.getMapId()==t.getMapId()).findFirst().orElse(null);
                if(item!=null){
                    var dto=new NewRecordDto();
                    dto.setMapId(t.getMapId());
                    dto.setMapName(t.getMapName());
                    var player=new NewRecordDto_Player();
                    player.setPlayerId(t.getPlayerId());
                    player.setPlayerName(t.getPlayerName());
                    player.setStage(t.getStage());
                    player.setDate(t.getDate());
                    player.setTime(t.getTime());
                    dto.setPlayers(new ArrayList<>());
                    result.add(dto);
                }
                else {
                    var player=new NewRecordDto_Player();
                    player.setPlayerId(t.getPlayerId());
                    player.setPlayerName(t.getPlayerName());
                    player.setStage(t.getStage());
                    player.setDate(t.getDate());
                    player.setTime(t.getTime());
                    item.getPlayers().add(player);
                }
                if(result.size()==11){
                    result.remove(result.size()-1);
                    isQuit=true;
                    break;
                }
            }
            if (isQuit) break;
        }
        //查询地图信息
        var mapInfo= mapMapper.getMapInfoByIds(result.stream().map(NewRecordDto::getMapId).collect(Collectors.toList()));
        var mapWrList =this.getMapWrList(recordType).stream().filter(t->result.stream().anyMatch(a->t.getMapId()==t.getMapId())).collect(Collectors.toList());
        //填充地图信息
        for (var item:result) {
            var map = mapInfo.stream().filter(t -> t.getId() == item.getMapId()).findFirst().orElse(null);
            if (map != null) {
                item.setDifficulty(map.getDifficulty());
                item.setImg(map.getImg());
            }
            for (var t:item.getPlayers()) {
                t.setGapTime(mapWrList.stream().anyMatch(b->b.getMapId()==item.getMapId()&&b.getStage()==t.getStage())?
                        mapWrList.stream().filter(b->b.getMapId()==item.getMapId()&&b.getStage()==t.getStage()).map(b->b.getTime()).findFirst().get()-t.getTime():-1);
            }
            item.setPlayers(item.getPlayers().stream().sorted(Comparator.comparing(NewRecordDto_Player::getPlayerName).thenComparing(NewRecordDto_Player::getStage)).collect(Collectors.toList()));
        }
        return result;
    }

    @Override
    public List<NewMapDto> getNewMapList() {
        return mapMapper.getNewMapList();
    }

    @Override
    public List<PopularMapDto> getPopularMapList() {
        return mapMapper.getPopularMapList();
    }
}
