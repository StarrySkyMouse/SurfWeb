<template>
  <el-row>
    <el-col :xs="24" :sm="12">
      <div class="app-container">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>积分</span>
          </div>
          <el-table v-loading="integralListLoading" :data="integralList" element-loading-text="Loading"
            borderfithighlight-current-row>
            <el-table-column align="center" label="排名" width="100">
              <template slot-scope="scope">
                {{ scope.$index + 1 }}
              </template>
            </el-table-column>
            <el-table-column align="center" label="玩家">
              <template slot-scope="scope">
                <el-link type="primary" @click="openPlayer(scope.row)">{{ scope.row.playerName }}</el-link>
              </template>
            </el-table-column>
            <el-table-column align="center" label="积分" width="100">
              <template slot-scope="scope">
                {{ scope.row.value }}
              </template>
            </el-table-column>
            <el-table-column align="center" label="地图完成度" width="100">
              <template slot-scope="scope">
                {{ scope.row.progress }}
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </div>
    </el-col>
    <el-col :xs="24" :sm="12">
      <div class="app-container">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>WR</span>
          </div>
          <el-row>
            <el-col :span="24">
              <el-radio-group v-model="recordType" @input="changeWrBtn()">
                <el-radio-button label="1"> 主线</el-radio-button>
                <el-radio-button label="2">奖励</el-radio-button>
                <el-radio-button label="3">阶段</el-radio-button>
              </el-radio-group>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <el-table v-loading="wRListLoading" :data="wRList" element-loading-text="Loading"
                borderfithighlight-current-row>
                <el-table-column align="center" label="#" width="100">
                  <template slot-scope="scope">
                    {{ scope.$index + 1 }}
                  </template>
                </el-table-column>
                <el-table-column align="center" label="玩家">
                  <template slot-scope="scope">
                    <el-link type="primary" @click="openPlayer(scope.row)">{{ scope.row.playerName }}</el-link>
                  </template>
                </el-table-column>
                <el-table-column align="center" label="数量" width="100">
                  <template slot-scope="scope">
                    {{ scope.row.value | intValue }}
                  </template>
                </el-table-column>
              </el-table></el-col>
          </el-row>
        </el-card>
      </div>
    </el-col>
  </el-row>
</template>

<script>
import { getRankingList } from '@/api/ranking'

export default {
  name: 'Ranking',
  filters: {
    intValue(val) {
      // 转为整数
      return parseInt(val, 10)
    }
  },
  data() {
    return {
      recordType: 1,
      // 积分
      integralList: [],
      integralListLoading: true,
      // 积分
      wRList: [],
      wRListLoading: true
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    changeWrBtn() {
      this.wRListLoading = true
      getRankingList({ rankingType: this.recordType }).then(response => {
        this.wRList = response.data
        this.wRListLoading = false
      })
    },
    openPlayer(row) {
      this.$router.push({
        path: `/player/${row.playerId}`,
        query: { tagName: row.playerName }
      })
    },
    fetchData() {
      this.integralListLoading = true
      getRankingList({ rankingType: 0 }).then(response => {
        this.integralList = response.data
        this.integralListLoading = false
      })
      this.changeWrBtn()
    }
  }
}
</script>
<style lang="scss" scoped>
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
}
</style>
