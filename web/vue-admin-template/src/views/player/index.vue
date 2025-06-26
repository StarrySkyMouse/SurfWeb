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
          <el-descriptions class="margin-top" :title="playerInfo.name" :column="isMobile ? 2 : 3" :size="size">
            <el-descriptions-item label="积分排名">
              {{ playerInfo.integralRanking }}({{ playerInfo.integral }})
            </el-descriptions-item>
            <el-descriptions-item label="主线完成排名">
              {{ playerInfo.succeesRanking }}/({{ playerInfo.swrNumber }})
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
            <span>WR</span>
          </div>
          <el-row>
            <el-col :span="24">
              <el-radio-group v-model="wrRecordType" @input="changeWrBtn()">
                <el-radio-button label="0">主线</el-radio-button>
                <el-radio-button label="1">奖励</el-radio-button>
                <el-radio-button label="2">阶段</el-radio-button>
              </el-radio-group>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <el-table v-loading="playerWRListLoading" :data="playerWRList" element-loading-text="Loading"
                borderfithighlight-current-row>
                <el-table-column align="center" label="#">
                  <template slot-scope="scope">
                    {{ scope.$index + 1 }}
                  </template>
                </el-table-column>
                <el-table-column align="center" label="地图">
                  <template slot-scope="scope">
                    <el-link type="primary" @click="openMap(scope.row)">{{ scope.row.mapName }}</el-link>
                  </template>
                </el-table-column>
                <el-table-column align="center" label="时间">
                  <template slot-scope="scope">
                    {{ scope.row.time }}
                  </template>
                </el-table-column>
                <el-table-column align="center" label="日期">
                  <template slot-scope="scope">
                    {{ scope.row.date }}
                  </template>
                </el-table-column>
              </el-table>
            </el-col>
          </el-row>
          <el-pagination style="text-align: center;" :current-page="playerWRpageIndex" small :page-sizes="[10]"
            layout="prev, pager, next" :total="playerWRTotal" @current-change="handlePlayerWRChange">
          </el-pagination>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12">
        <el-card class="box-card data-card">
          <div slot="header" class="clearfix">
            <span>已完成</span>
          </div>
          <el-row>
            <el-col :span="24">
              <el-radio-group v-model="succeessRecordType" @input="changeSucceesBtn()">
                <el-radio-button label="0">主线</el-radio-button>
                <el-radio-button label="1">奖励</el-radio-button>
                <el-radio-button label="2">阶段</el-radio-button>
              </el-radio-group>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <el-table v-loading="succeesListLoading" :data="succeesList" element-loading-text="Loading"
                borderfithighlight-current-row>
                <el-table-column align="center" label="#">
                  <template slot-scope="scope">
                    {{ scope.$index + 1 }}
                  </template>
                </el-table-column>
                <el-table-column align="center" label="地图">
                  <template slot-scope="scope">
                    <el-link type="primary" @click="openMap(scope.row)">{{ scope.row.mapName }}</el-link>
                  </template>
                </el-table-column>
                <el-table-column align="center" label="时间">
                  <template slot-scope="scope">
                    {{ scope.row.time }}
                  </template>
                </el-table-column>
                <el-table-column align="center" label="日期">
                  <template slot-scope="scope">
                    {{ scope.row.date }}
                  </template>
                </el-table-column>
              </el-table>
            </el-col>
          </el-row>
          <el-pagination style="text-align: center;" :current-page="succeesPageIndex" small :page-sizes="[10]"
            layout="prev, pager, next" :total="succeesTotal" @current-change="handleSucceesChange">
          </el-pagination>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12">
        <el-card class="box-card data-card">
          <div slot="header" class="clearfix">
            <span>未完成</span>
          </div>
          <el-row>
            <el-col :span="24">
              <el-radio-group v-model="failRecordType" @input="changeFailBtn()">
                <el-radio-button label="0">主线</el-radio-button>
                <el-radio-button label="1">奖励</el-radio-button>
                <el-radio-button label="2">阶段</el-radio-button>
              </el-radio-group>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <el-table v-loading="failListLoading" :data="failList" element-loading-text="Loading"
                borderfithighlight-current-row>
                <el-table-column align="center" label="#">
                  <template slot-scope="scope">
                    {{ scope.$index + 1 }}
                  </template>
                </el-table-column>
                <el-table-column align="center" label="地图">
                  <template slot-scope="scope">
                    <el-link type="primary" @click="openMap(scope.row)">{{ scope.row.mapName }}</el-link>
                  </template>
                </el-table-column>
                <el-table-column align="center" label="阶段" v-if="failRecordType != 0">
                  <template slot-scope="scope">
                    {{ scope.row.stage }}
                  </template>
                </el-table-column>
              </el-table>
            </el-col>
          </el-row>
          <el-pagination style="text-align: center;" :current-page="failPageIndex" small :page-sizes="[10]"
            layout="prev, pager, next" :total="failTotal" @current-change="handleFailChange">
          </el-pagination>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import {
  getPlayerInfo,
  getPlayerWRCount,
  getPlayerWRList,
  getPlayerSucceesCount,
  getPlayerSucceesList,
  getPlayerFailCount,
  getPlayerFailList
} from '@/api/player'

export default {
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'gray',
        deleted: 'danger'
      }
      return statusMap[status]
    }
  },
  data() {
    return {
      isMobile: window.innerWidth < 768,
      playerInfoLoading: true,
      playerWRListLoading: true,
      playerWRList: null,
      succeesListLoading: true,
      succeesList: null,
      failListLoading: true,
      failList: null,
      playerId: null,
      playerInfo: {},
      wrRecordType: 0,
      succeessRecordType: 0,
      failRecordType: 0,
      playerWRpageIndex: 1,
      playerWRTotal: 0,
      succeesPageIndex: 1,
      succeesTotal: 0,
      failPageIndex: 1,
      failTotal: 0
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
    handleResize() {
      this.isMobile = window.innerWidth < 768
    },
    fetchData() {
      this.getPlayerInfo()
      this.getPlayerWRCount()
      this.getPlayerWRList()
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
    getPlayerWRCount() {
      getPlayerWRCount({ id: this.playerId, recordType: this.wrRecordType }).then(response => {
        this.playerWRTotal = response.data
      })
    },
    getPlayerWRList() {
      this.playerWRListLoading = true
      getPlayerWRList({ id: this.playerId, recordType: this.wrRecordType, pageIndex: this.playerWRpageIndex }).then(response => {
        this.playerWRList = response.data
        this.playerWRListLoading = false
      })
    },
    getPlayerSucceesCount() {
      getPlayerSucceesCount({ id: this.playerId, recordType: this.succeessRecordType }).then(response => {
        this.succeesTotal = response.data
      })
    },
    getPlayerSucceesList() {
      this.succeesListLoading = true
      getPlayerSucceesList({ id: this.playerId, recordType: this.succeessRecordType, pageIndex: this.succeesPageIndex }).then(response => {
        this.succeesList = response.data
        this.succeesListLoading = false
      })
    },
    getPlayerFailCount() {
      getPlayerFailCount({ id: this.playerId, recordType: this.failRecordType }).then(response => {
        this.failTotal = response.data
      })
    },
    getPlayerFailList() {
      this.failListLoading = true
      getPlayerFailList({ id: this.playerId, recordType: this.failRecordType, pageIndex: this.failPageIndex }).then(response => {
        this.failList = response.data
        this.failListLoading = false
      })
    },
    handlePlayerWRChange(val) {
      this.playerWRpageIndex = val
      this.getPlayerWRList()
    },
    handleSucceesChange(val) {
      this.succeesPageIndex = val
      this.getPlayerSucceesList()
    },
    handleFailChange(val) {
      this.failPageIndex = val
      this.getPlayerFailList()
    },
    changeWrBtn() {
      this.playerWRpageIndex = 1
      this.getPlayerWRCount()
      this.getPlayerWRList()
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
}
</style>
