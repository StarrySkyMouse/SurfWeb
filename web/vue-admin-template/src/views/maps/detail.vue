<template>
  <div class="app-container">
    <el-card class="box-card head">
      <div class="head-wrapper">
        <div class="head-img-wrapper">
          <div class="image-wrapper">
            <img class="image" :src="mapInfo.img">
          </div>
        </div>
        <div class="head-content-wrapper">
          <el-descriptions class="margin-top" :title="mapInfo.name" :column="3">
            <el-descriptions-item label="难度">{{ mapInfo.difficulty }}</el-descriptions-item>
            <el-descriptions-item label="完成人数">{{ mapInfo.surcessNumber }}</el-descriptions-item>
            <el-descriptions-item label="奖励关">{{ mapInfo.bonusNumber }}</el-descriptions-item>
            <el-descriptions-item label="阶段关数">{{ mapInfo.stageNumber }}</el-descriptions-item>
          </el-descriptions>
        </div>
      </div>
    </el-card>
    <el-card class="box-card data-card">
      <div slot="header" class="clearfix">
        <span>Top100</span>
      </div>
      <el-row>
        <el-col :span="24">
          <el-radio-group v-model="recordType" @input="changeWrBtn()">
            <el-radio-button label="0"> 主线</el-radio-button>
            <el-radio-button label="1">奖励</el-radio-button>
            <el-radio-button label="2">阶段</el-radio-button>
          </el-radio-group>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="24">
          <Tabs :tabs="tabs" @tabChange="failTabChange">
            <el-table v-loading="listLoading" :data="data" element-loading-text="Loading"
              borderfithighlight-current-row>
              <el-table-column align="center" label="#" width="80">
                <template slot-scope="scope">
                  {{ scope.row.ranking }}
                </template>
              </el-table-column>
              <el-table-column align="center" label="玩家">
                <template slot-scope="scope">
                  <el-link type="primary" @click="openPlayer(scope.row)">{{ scope.row.playerName }}</el-link>
                </template>
              </el-table-column>
              <el-table-column align="center" :label="recordType != 0 ? '阶段' : ''" width="60">
                <template slot-scope="scope">
                  {{ scope.row.stage }}
                </template>
              </el-table-column>
              <el-table-column align="center" label="时间">
                <template slot-scope="scope">
                  {{ scope.row.time | FormattingTime }}<template v-if="scope.row.ranking != 1">(+{{ scope.row.gapTime
                    |
                    FormattingTime
                  }})</template>
                </template>
              </el-table-column>
              <el-table-column align="center" label="日期">
                <template slot-scope="scope">
                  {{ scope.row.date }}
                </template>
              </el-table-column>
            </el-table>
            <el-pagination v-show="!isMobile" layout="total, sizes, prev, pager, next, jumper" :current-page="pageIndex"
              :page-sizes="[10]" :total="total" @current-change="handleCurrentChange" />
            <el-pagination style="text-align: center;" v-show="isMobile" :current-page="pageIndex" small
              :page-sizes="[10]" layout="prev, pager, next" :total="total" @current-change="handleCurrentChange">
            </el-pagination>
          </Tabs>
        </el-col>
      </el-row>
    </el-card>
  </div>
</template>
<script>
import { getMapTop100Count, getMapTop100List, getMapInfo } from '@/api/maps'
import Tabs from '@/components/Tabs'

export default {
  name: 'Detail',
  components: {
    Tabs
  },
  filters: {
    FormattingTime(val) {
      return val.toString().replace('00:', '').replace('00:', '')
    }
  },
  data() {
    return {
      recordType: '0',
      mapInfo: {},
      data: [],
      tabs: [1],
      tabActive: 1,
      isMobile: window.innerWidth < 768,
      radio1: '全部',
      list: null,
      listLoading: true,
      top100Count: 0,
      top100List: [],
      total: 0,
      pageIndex: 1,
      mapId: null
    }
  },
  created() {
    this.mapId = this.$route.params.id
    this.fetchData()
  },
  mounted() {
    window.addEventListener('resize', this.handleResize)
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.handleResize)
  },
  methods: {
    openPlayer(row) {
      this.$router.push({
        path: `/player/${row.playerId}`,
        query: { tagName: row.playerName }
      })
    },
    fetchData() {
      getMapInfo({ id: this.mapId }).then(response => {
        this.mapInfo = response.data
        if (!this.mapInfo) {
          this.$router.push({ path: `/404` })
          return
        }
        this.getTop100Count()
        this.getTop100Data()
      })
    },
    handleResize() {
      this.isMobile = window.innerWidth < 768
    },
    getTop100Count() {
      getMapTop100Count({ id: this.mapId, recordType: this.recordType, stage: this.tabActive }).then(response => {
        this.total = response.data
      })
    },
    getTop100Data() {
      this.listLoading = true
      getMapTop100List({ id: this.mapId, recordType: this.recordType, stage: this.tabActive, pageIndex: this.pageIndex }).then(response => {
        this.data = response.data
        this.listLoading = false
      })
    },
    changeWrBtn() {
      if (this.recordType === '0') {
        this.tabs = [1]
      } else if (this.recordType === '1') {
        this.tabs = this.mapInfo.bonusNumber > 0 ? Array.from({ length: this.mapInfo.bonusNumber }, (_, i) => i + 1) : [1]
      } else {
        this.tabs = this.mapInfo.stageNumber > 0 ? Array.from({ length: this.mapInfo.stageNumber }, (_, i) => i + 1) : [1]
      }
      this.failTabChange(0)
    },
    handleCurrentChange(val) {
      this.pageIndex = val
      this.getTop100Data()
    },
    failTabChange(idx) {
      this.tabActive = idx + 1
      document.querySelectorAll('.tab-item').forEach((el, i) => {
        if (i === idx) {
          el.classList.add('active')
        } else {
          el.classList.remove('active')
        }
      })
      this.pageIndex = 1
      this.getTop100Count()
      this.getTop100Data()
    }
  }
}
</script>
<style lang="scss" scoped>
::v-deep {

  .el-card {
    margin-bottom: 10px;
  }

  .head {
    border-left: 10px solid #EA2F14;
  }

  @media (max-width: 768px) {
    .head-wrapper {
      display: block !important;
      align-items: initial !important;
    }
  }

  .head-wrapper {
    display: flex;
    align-items: flex-start;

    .head-img-wrapper {
      min-width: 350px;
    }

    .head-content-wrapper {
      margin-left: 20px;
      margin-top: 10px;
    }
  }

  .image-wrapper {
    position: relative;
    width: 100%;
    padding-top: 56.25%; // 16:9 = 9/16 = 0.5625
    overflow: hidden;
  }

  .image {
    position: absolute;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    object-fit: cover;
    display: block;
  }

}
</style>
