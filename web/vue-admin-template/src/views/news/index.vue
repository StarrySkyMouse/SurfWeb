<template>
  <el-row>
    <el-col :xs="24" :sm="14">
      <div class="app-container">
        <el-card class="box-card">
          <div slot="header" class="clearfix" style="line-height: 32px;">
            <span>新纪录</span>
          </div>
          <el-row>
            <el-col :span="24">
              <el-radio-group v-model="recordType" @input="getNewRecordData()">
                <el-radio-button label="0">主线</el-radio-button>
                <el-radio-button label="1">奖励</el-radio-button>
                <el-radio-button label="2">阶段</el-radio-button>
              </el-radio-group>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <el-table v-loading="newRecordLoading" :data="newRecordList" element-loading-text="Loading">
                <el-table-column align="center" width="300">
                  <template slot-scope="scope">
                    <MapCard :mapInfo="scope.row" @openMap="openMap" />
                  </template>
                </el-table-column>
                <el-table-column>
                  <template slot-scope="scope">
                    <el-table :data="scope.row.players" size="small" border
                      style="margin-top: 10px;max-height: 800px;overflow:auto;">
                      <el-table-column align="center" :label="recordTypeLoading == '1' ? '奖励' : '阶段'"
                        v-if="recordTypeLoading != 0" width="100">
                        <template slot-scope="scope">
                          {{ scope.row.stage }}
                        </template>
                      </el-table-column>
                      <el-table-column align="center" label="玩家">
                        <template slot-scope="scope">
                          <el-link type="primary" @click="openPlayer(scope.row)">{{ scope.row.playerName }}</el-link>
                        </template>
                      </el-table-column>
                      <el-table-column align="center" label="时间" width="220">
                        <template slot-scope="scope">
                          {{ scope.row.time }}
                          <span class="wrLable" v-if="scope.row.gapTime == '00:00:00.0000'">wr</span>
                          <template v-else>
                            (+{{ scope.row.gapTime | FormattingTime }})
                          </template>
                        </template>
                      </el-table-column>
                      <el-table-column align="center" label="日期" width="90">
                        <template slot-scope="scope">
                          {{ scope.row.date }}
                        </template>
                      </el-table-column>
                    </el-table>
                  </template>
                </el-table-column>
              </el-table></el-col>
          </el-row>
        </el-card>
      </div>
    </el-col>
    <el-col :xs="24" :sm="10" class="fixed-right-col">
      <div class="app-container">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <div class="radio-inputs">
              <label class="radio">
                <input type="radio" name="mapTab" value="popular" v-model="mapTab" @change="handleMapTabChange">
                <span class="name">热门地图</span>
              </label>
              <label class="radio">
                <input type="radio" name="mapTab" value="new" v-model="mapTab" @change="handleMapTabChange">
                <span class="name">新地图</span>
              </label>
            </div>
          </div>
          <el-table v-if="mapTab === 'popular'" v-loading="popularMapLoading" :data="popularMapList"
            element-loading-text="Loading">
            <el-table-column align="center" width="300">
              <template slot-scope="scope">
                <MapCard :mapInfo="scope.row" @openMap="openMap" />
              </template>
            </el-table-column>
            <el-table-column align="center" label="完成人数">
              <template slot-scope="scope">
                <span>{{ scope.row.surcessNumber }}</span>
              </template>
            </el-table-column>
          </el-table>
          <el-table v-else v-loading="newMapLoading" :data="newMapList" element-loading-text="Loading">
            <el-table-column align="center" width="300">
              <template slot-scope="scope">
                <MapCard :mapInfo="scope.row" @openMap="openMap" />
              </template>
            </el-table-column>
            <el-table-column align="center" label="日期">
              <template slot-scope="scope">
                {{ scope.row.createTime }}
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </div>
    </el-col>
  </el-row>
</template>

<script>
import { getNewRecordList, getNewMapList, getPopularMapList } from '@/api/news'
import MapCard from '@/components/MapCard'

export default {
  name: 'NewsIndex',
  components: {
    MapCard
  },
  data() {
    return {
      recordType: 0,
      recordTypeLoading: 0,
      newRecordList: null,
      newRecordLoading: true,
      newMapList: null,
      newMapLoading: true,
      popularMapList: null,
      popularMapLoading: true,
      mapTab: 'popular' // 默认显示热门地图
    }
  },
  filters: {
    FormattingTime(val) {
      return val.toString().replace('00:', '').replace('00:', '')
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    handleMapTabChange() {
      if (this.mapTab === 'popular') {
        this.getPopularMapData()
      } else if (this.mapTab === 'new') {
        this.getNewMapData()
      }
    },
    openPlayer(row) {
      this.$router.push({
        path: `/player/${row.playerId}`,
        query: { tagName: row.playerName }
      })
    },
    openMap(row) {
      console.log(row, 'row')
      this.$router.push({
        path: `/maps/detail/${row.id == null ? row.mapId : row.id}`,
        query: { tagName: row.name == null ? row.mapName : row.name }
      })
    },
    getNewRecordData() {
      this.newRecordLoading = true
      getNewRecordList({ recordType: this.recordType }).then(response => {
        this.newRecordList = response.data
        this.recordTypeLoading = this.recordType
        setTimeout(() => {
          this.newRecordLoading = false
        }, 300)
      })
    },
    getNewMapData() {
      this.newMapLoading = true
      getNewMapList().then(response => {
        this.newMapList = response.data
        setTimeout(() => {
          this.newMapLoading = false
        }, 300)
      })
    },
    getPopularMapData() {
      this.popularMapLoading = true
      getPopularMapList().then(response => {
        this.popularMapList = response.data
        setTimeout(() => {
          this.popularMapLoading = false
        }, 300)
      })
    },
    fetchData() {
      this.getNewRecordData()
      this.getNewMapData()
      this.getPopularMapData()
    }
  }
}
</script>
<style lang="scss" scoped>

.radio-inputs {
  position: relative;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -ms-flex-wrap: wrap;
  flex-wrap: wrap;
  border-radius: 0.4rem;
  background-color: #EEE;
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
  width: 300px;
  font-size: 15px;
}

.radio-inputs .radio {
  flex: 1 1 auto;
  text-align: center;
}

.radio-inputs .radio input {
  display: none;
}

.radio-inputs .radio .name {
  display: flex;
  cursor: pointer;
  align-items: center;
  justify-content: center;
  border-radius: 0.5rem;
  border: none;
  padding: .5rem 0;
  color: rgba(51, 65, 85, 1);
  transition: all .15s ease-in-out;
}

.radio-inputs .radio input:checked+.name {
  color: #000;
  background-color: #fff;
  font-weight: 900;
}

.el-row {
  margin: 20px;
}

::v-deep {
  .el-card__body {
    padding: 0px;
  }

  .app-container {
    padding: 10px;
  }

  @media (max-width: 768px) {
    .app-container {
      padding: 0px;
    }
  }

  .el-loading-spinner {
    top: 100px !important;
    /* 距顶部40px，可根据需要调整 */
    bottom: auto !important;
    left: 50%;
    transform: translateX(-50%);
    position: absolute;
  }
}
</style>
