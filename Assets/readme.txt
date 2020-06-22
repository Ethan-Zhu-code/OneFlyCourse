@Copyright (C) 2019 讯飞幻境（北京）科技有限公司
@Name ： OneFlySDK
@Author: RyuRae
@Version: 1.2.0
1.此sdk集成了课程控制和卡牌交互，使用时请将CourseControl挂在场景物体上，然后将场景中的卡牌物体拖到objs上，示例中ControlCenter实现上述操作
2.卡牌的UI需要附加CardControl脚本，或自己创建脚本继承UIParent
3.卡牌的上屏显示的物体需要附加ModelControl脚本，或者自己创建脚本继承LabObject
4.操作提示脚本OperationHint已集成
5.语音提示脚本AudioHint及语音播放脚本AudioPlayer均已集成，示例场景中有对应的物体

@Version: 1.3.0
1.此版本加入了课程信息类，添加了对课程信息的收集
2.添加了流程管理类，以及流程状态类，每个流程状态会有多个状态目标（stepGoal）集合，完成所有当前状态，才可以进入下个流程
3.状态目标，目前已有动画状态目标（AnimStepGoal）、音频状态目标(AudioStepGoal)、位置状态目标（PosStepGoal）；若有新的状态目标需继承StepGoal，然后附加到目标物体上
4.课程管理类（CourseControl）需要配置课程类型，目前课程分为四大类操作实验类（OPERATION）、科普类（COUPE）、观察现象类（OBSERVE）以及其他（OTHER）
5.原流程管理类StageControl同样可用，所有流程在AllStageCheck里实现
6.此版本合入了ManagerEvent消息管理机制

@Version: 1.4.0
1.根据反馈修改了操作提示的显示问题
2.添加了开始卡牌，以及UI提示的操作
3.添加单场景多实验添加，多实验选择时需要将CourseControl挂在第一场景，目的：此脚本为加载场景不销毁的单例，放在第一场景避免反复创建，发生错误。

@Version: 1.4.1
1.修改了部分bug
2.添加了实验总结--错误扣分UI
3.添加课程控制卡牌并实现功能

@Version: 1.4.2
1.添加了3D模型不同轴向的移动（XY、XZ、YZ、X、Y、Z）
2.将CourseControl中courseInfo变成静态,方便调用
3.修改开始界面UI显示，添加第三条操作提示

@Version: 1.4.3
1.修改部分bug；卡牌移除事件不能触发，显示提示时报错等问题
2.添加实验目的UI

@Version: 1.5.0
1.完善同一个课程有多个实验的问题，CourseControl中新开始一个实验需要调用Init方法，初始化实验
2.完善卡牌的移动方式，在xy平面上移动区分是否根据相机视角移动（isAccordCamera）,并在整体移动方式上做了优化，新增了增量移动
3.其中选择增量移动时需要初始化卡牌位置，调用ModelControl的InitDeltaVec方法，sdk中卡牌放入下屏时会默认初始化一次
4.统一实验器材提示标签，要求大小一致
5.实验总结提示框改为实验得分，要求同时且最多可以显示10条错误提示
6.课程最后的语音和文本框：”你可以放入课程控制卡牌退出课程或重新开始。“，统一不再显示，只读即可，和前面的开始卡牌处理方式一样
7.实验声音不特殊说明统一用女声配音
8.实验目的从实验提示中提出单独显示，多个实验的实验提示根据自己的情况实现UIScenePurpose.cs方法

@Version: 1.5.1
1.优化CardOptionControl中的方法，使方法更规范易于重写，并基于此修改课程控制卡牌
2.修改CardOptionControl中重复注册和注销的CardUpdate事件，并修改为CardRemove事件

@Version: 1.5.2
1.替换下屏UI（包括BG，四张通用卡牌）
2.增加2张通用卡牌的功能(第一人称视角、实验信息卡牌)
3.修改上屏收集卡牌界面，并修改UIScene_Purpose名称错误问题
4.修改CardControl中的ConvertAngleToOrientation（）方法，对卡牌的left和right进行判空
5.将Page0场景Hierarachy面板中ReceiverEventHandler重命名为cardLogicReceiver，卡牌逻辑接收器可放在cardLogicReceiver下
6.在AudioHint中增加单独播放语音不显示文字的方法：ShowHint(int indx)

@Version: 1.5.3
1.添加了ManagerEvent.Clear()方法，用于清空所有已注册的消息
2.修改了课程控制卡牌中的重新开始功能，在重新开始时，清空所有消息

@Version: 1.5.4
1.更新UI界面的文字大小及行间距
2.修改实验场景不是原点时单独在x/y/z轴无法移动的问题
3.修改xy界面不根据相机移动y轴坐标小于0点归零的问题
4.勾选isLimit后所有轴向移动都按轴向受限制值约束
5.重新导入DoTween插件，可以在物体上直接使用DoTweenAnimation动画

@Version: 1.5.5
1.修改流程控制StageControl中的流程初始化，填写实验步数即可实现自动初始化，而后实现每步流程的具体交互
2.ManagerEvent加入泛型方法注册机制，泛型方法调用Manager.Send<T>
3.修改优化增量移动
4.修改错误列表预制件，调整错误列表最多可同时显示10个错误
5.修改课程开场动画



