<template>
  <div class="app-container">
    <el-skeleton :loading="playerInfoLoading" animated>
      <template #template>
        <el-card class="box-card head">
          <div><el-skeleton-item variant="h3" style="width: 10%" /></div>
          <div><el-skeleton-item variant="h3" style="width: 90%" /></div>
          <div><el-skeleton-item variant="h3" style="width: 90%" /></div>
          <div><el-skeleton-item variant="h3" style="width: 90%" /></div>
          <div><el-skeleton-item variant="h3" style="width: 90%" /></div>
        </el-card>
      </template>
      <template #default>
        <!-- 原来的内容 -->
        <el-card class="box-card head">
          <el-descriptions class="margin-top" :title="playerInfo.name" :column="isMobile ? 2 : 3">
            <el-descriptions-item label="积分排名">
              {{ playerInfo.integralRanking }}({{ playerInfo.integral }})
            </el-descriptions-item>
            <el-descriptions-item label="主线完成排名">
              {{ playerInfo.succeesRanking }}/({{ playerInfo.succeesNumber }})
            </el-descriptions-item>
            <el-descriptions-item label="主线WR排名">
              {{ playerInfo.wrRanking }}({{ playerInfo.wrNumber }})
            </el-descriptions-item>
            <el-descriptions-item label="奖励WR排名">
              {{ playerInfo.bwRanking }}({{ playerInfo.bwrNumber }})
            </el-descriptions-item>
            <el-descriptions-item label="阶段WR排名">
              {{ playerInfo.swRanking }}({{ playerInfo.swrNumber }})
            </el-descriptions-item>
          </el-descriptions>
        </el-card>
      </template>
    </el-skeleton>
    <el-row>
      <el-col :xs="24" :sm="12">
        <el-card class="box-card data-card">
          <div slot="header" class="clearfix">
            <span>已完成</span>
          </div>
          <el-row>
            <el-col :span="24" class="data-card-bottom">
              <el-radio-group v-model="succeessRecordType" @input="changeSucceesBtn()">
                <el-radio-button label="0">主线</el-radio-button>
                <el-radio-button label="1">奖励</el-radio-button>
                <el-radio-button label="2">阶段</el-radio-button>
              </el-radio-group>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <Tabs :tabs="tabs" @tabChange="succeessTabChange">
                <el-table v-if="succeessRecordType == 0" v-loading="succeesListLoading" :data="succeesList">
                  <el-table-column align="center" label="地图" width="300">
                    <template slot-scope="scope">
                      <MapCard :mapInfo="scope.row" @openMap="openMap" />
                    </template>
                  </el-table-column>
                  <el-table-column align="center" label="时间">
                    <template slot-scope="scope">
                      {{ scope.row.stages[0].time }}
                      <span class="wrLable" v-if="scope.row.stages[0].gapTime == '00:00:00.0000'">wr</span>
                      <template v-else>
                        (+{{ scope.row.stages[0].gapTime | FormattingTime }})
                      </template>
                    </template>
                  </el-table-column>
                  <el-table-column align="center" label="日期">
                    <template slot-scope="scope">
                      {{ scope.row.stages[0].date }}
                    </template>
                  </el-table-column>
                </el-table>
                <el-table v-if="succeessRecordType != 0" v-loading="succeesListLoading" :data="succeesList"
                  style="width: 100%">
                  <el-table-column align="center" label="地图" width="300">
                    <template slot-scope="scope">
                      <MapCard :mapInfo="scope.row" @openMap="openMap" />
                    </template>
                  </el-table-column>
                  <el-table-column label="阶段详情">
                    <template slot-scope="scope">
                      <el-table :data="scope.row.stages" size="small" border style="margin-top: 10px;">
                        <el-table-column prop="stage" align="center" :label="succeessRecordType == '1' ? '奖励' : '阶段'"
                          width="60" />
                        <el-table-column align="center" label="时间">
                          <template slot-scope="scope">
                            {{ scope.row.time }}
                            <el-tag v-if="scope.row.gapTime == '00:00:00.0000'" effect="plain" type="danger"
                              size="mini">WR</el-tag>
                            <template v-else>
                              (+{{ scope.row.gapTime | FormattingTime }})
                            </template>
                          </template>
                        </el-table-column>
                        <el-table-column align="center" label="日期">
                          <template slot-scope="scope">
                            {{ scope.row.date }}
                          </template>
                        </el-table-column>
                      </el-table>
                    </template>
                  </el-table-column>
                </el-table>
                <el-pagination style="text-align: center;" :current-page="succeesPageIndex" small :page-sizes="[10]"
                  layout="prev, pager, next" :total="succeesTotal" @current-change="handleSucceesChange">
                </el-pagination>
              </Tabs>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12">
        <el-card class="box-card data-card">
          <div slot="header" class="clearfix">
            <span>未完成</span>
          </div>
          <el-row>
            <el-col :span="24" class="data-card-bottom">
              <el-radio-group v-model="failRecordType" @input="changeFailBtn()">
                <el-radio-button label="0">主线</el-radio-button>
                <el-radio-button label="1">奖励</el-radio-button>
                <el-radio-button label="2">阶段</el-radio-button>
              </el-radio-group>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <Tabs :tabs="tabs" @tabChange="failTabChange">
                <el-table v-loading="failListLoading" :data="failList">
                  <el-table-column align="center" label="地图" width="300">
                    <template slot-scope="scope">
                      <MapCard :mapInfo="scope.row" @openMap="openMap" />
                    </template>
                  </el-table-column>
                  <el-table-column align="center" :label="''">
                    <template slot-scope="scope" v-if="failRecordType != 0">
                      <el-table :data="scope.row.stages" size="small" border style="margin-top: 10px;">
                        <el-table-column align="center" :label="failRecordType == '1' ? '奖励' : '阶段'">
                          <template slot-scope="scope">
                            {{ scope.row }}
                          </template>
                        </el-table-column>
                      </el-table>
                    </template>
                  </el-table-column>
                </el-table>
                <el-pagination style="text-align: center;" :current-page="failPageIndex" small :page-sizes="[10]"
                  layout="prev, pager, next" :total="failTotal" @current-change="handleFailChange">
                </el-pagination>
              </Tabs>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import {
  getPlayerInfo,
  getPlayerSucceesCount,
  getPlayerSucceesList,
  getPlayerFailCount,
  getPlayerFailList
} from '@/api/player'

import Tabs from '@/components/Tabs'
import MapCard from '@/components/MapCard'

export default {
  name: 'Player',
  filters: {
    FormattingTime(val) {
      return val.toString().replace('00:', '').replace('00:', '')
    }
  },
  components: {
    Tabs,
    MapCard
  },
  data() {
    return {
      isMobile: window.innerWidth < 768,
      playerInfoLoading: true,
      succeesListLoading: true,
      succeesList: null,
      failListLoading: true,
      failList: null,
      playerId: null,
      playerInfo: {},
      succeessRecordType: 0,
      failRecordType: 0,
      succeesPageIndex: 1,
      succeesTotal: 0,
      failPageIndex: 1,
      failTotal: 0,
      tabs: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'T8', 'T0'],
      succeessDifficulty: 'T1',
      failDifficulty: 'T1'
    }
  },
  created() {
    this.playerId = this.$route.params.id
    this.fetchData()
  },
  mounted() {
    window.addEventListener('resize', this.handleResize)
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.handleResize)
  },
  methods: {
    succeessTabChange(index) {
      this.succeesPageIndex = 1
      this.succeessDifficulty = this.tabs[index]
      this.getPlayerSucceesCount()
      this.getPlayerSucceesList()
    },
    failTabChange(index) {
      this.failPageIndex = 1
      this.failDifficulty = this.tabs[index]
      this.getPlayerFailCount()
      this.getPlayerFailList()
    },
    handleResize() {
      this.isMobile = window.innerWidth < 768
    },
    fetchData() {
      this.getPlayerInfo()
      this.getPlayerSucceesCount()
      this.getPlayerSucceesList()
      this.getPlayerFailCount()
      this.getPlayerFailList()
    },
    getPlayerInfo() {
      this.playerInfoLoading = true
      getPlayerInfo({ id: this.playerId }).then(response => {
        this.playerInfo = response.data
        this.playerInfoLoading = false
      })
    },
    getPlayerSucceesCount() {
      getPlayerSucceesCount({ id: this.playerId, recordType: this.succeessRecordType, difficulty: this.succeessDifficulty }).then(response => {
        this.succeesTotal = response.data
      })
    },
    getPlayerSucceesList() {
      this.succeesListLoading = true
      getPlayerSucceesList({ id: this.playerId, recordType: this.succeessRecordType, difficulty: this.succeessDifficulty, pageIndex: this.succeesPageIndex }).then(response => {
        this.succeesList = response.data
        this.succeesListLoading = false
      })
    },
    getPlayerFailCount() {
      getPlayerFailCount({ id: this.playerId, recordType: this.failRecordType, difficulty: this.failDifficulty }).then(response => {
        this.failTotal = response.data
      })
    },
    getPlayerFailList() {
      this.failListLoading = true
      getPlayerFailList({ id: this.playerId, recordType: this.failRecordType, difficulty: this.failDifficulty, pageIndex: this.failPageIndex }).then(response => {
        this.failList = response.data
        this.failListLoading = false
      })
    },
    handleSucceesChange(val) {
      this.succeesPageIndex = val
      this.getPlayerSucceesList()
    },
    handleFailChange(val) {
      this.failPageIndex = val
      this.getPlayerFailList()
    },
    changeSucceesBtn() {
      this.succeesPageIndex = 1
      this.getPlayerSucceesCount()
      this.getPlayerSucceesList()
    },
    changeFailBtn() {
      this.failPageIndex = 1
      this.getPlayerFailCount()
      this.getPlayerFailList()
    },
    openMap(row) {
      this.$router.push({
        path: `/maps/detail/${row.mapId}`,
        query: { tagName: row.mapName }
      })
    }
  }
}
</script>
<style lang="scss" scoped>
::v-deep {
  .el-card {
    margin: 10px;
  }

  .head {
    border-left: 10px solid #EA2F14;
  }

  .data-card {
    .el-card__body {
      padding: 0px;
    }
  }

  .tabs-wrapper {
    margin-top: 10px;
  }

  .data-card-bottom {
    margin-top: 10px;
    margin-left: 10px;
  }

}
</style>
