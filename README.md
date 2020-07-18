# :speech_balloon: Healthee Chatbot Service :runner:

### Topic: Hometraining을 도와줄 Azure based chatbot 
홈 트레이닝 챗봇 <b>Healthee</b>는 <p>
홈트레이닝을 하려는 분들에게  <p>
<b>운동을 추천</b>해주고 <b>운동 기구, 음식에 대한 정보</b>를 제공하여 <p>
운동을 도와드리는 것을 목표하고 있습 니다. 

### TEAM: Chatee
<img src="https://user-images.githubusercontent.com/41981471/86508101-52734500-be18-11ea-90e0-92df415e79d2.JPG" width="50%">



## 과제
[가이드 페이지](https://blog.naver.com/formktmkt/221994807603)

### 1. Gate 1

[Gate1 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/Gate1/Gate1.md)

### 2. Gate 2

[Gate2 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/Gate2/Gate2.md)


### 3. Gate 3 

[Gate 3 제출물, 결과 설명]()


## 소스코드 설명(2020-07-18 ver)
소스 구조 설명 및 핵심 코드 설명

### 구조
* Bots
** DialogAndWelcomeBot.cs : 새로운 사용자가 들어왔을 때 인사하는 부분. Echobot을 상속받았다.
** EchoBot.cs : 기존의 Echobot 변형. 사용자의 입력을 처리한다.

* Dialogs
** MainDialog.cs : 가장 상위 Dialog. TutorialDialog를 실행시킨다.
** TutorialDialog.cs : 튜토리얼을 진행하는 Dialog. Waterfall Dialog를 이용하여 질문과 답변이 순차적으로 이루어질 수 있게 구현하였다.

* UserProfile.cs : 사용자의 정보를 담을 클래스 선언
* Startup.cs : DialogAndWelcomeBot를 MainDialog로 실행시킨다.

### 코드 설명

#### Bots

##### `Bots/DialogAndWelcomeBot.cs`

![1](https://user-images.githubusercontent.com/41438361/87849805-76f61380-c926-11ea-9390-325600857f96.JPG)

DialogAndWelcomeBot은 EchoBot을 상속받았습니다.

![1](https://user-images.githubusercontent.com/41438361/87849847-cb998e80-c926-11ea-99b0-37b39393a2e9.JPG)

`OnMembersAddedAsync` 함수에서 사용자가 처음 봇과 대화를 시작할 때 인사를 합니다.

![2](https://user-images.githubusercontent.com/41438361/87849857-f552b580-c926-11ea-8981-7142bbad2da7.JPG)

`OnMembersAddedAsync` 함수에서 호출되는 `DisplayOptionsAsync` 함수에서 HeroCard를 이용하여 이미지를 보여줍니다.

#### `Bots/EchoBot.cs`

![3](https://user-images.githubusercontent.com/41438361/87849911-3f3b9b80-c927-11ea-87d3-8efd9fb375a4.JPG)

`OnMessageAddedAsync`에서는 먼저 QnaMaker를 불러옵니다.

![4](https://user-images.githubusercontent.com/41438361/87849946-7447ee00-c927-11ea-9d05-74115c1bb533.JPG)

그 후 `MainDialog.cs`에 있는 int type의 `tutorial` 변수를 참고하여 

`tutorial`이 0이면 tutorial이 아직 진행되지 않았다는 의미로 `Dialog.RunAsync` 함수로 튜토리얼을 진행하고, 
1이면 QnAMaker가 사용자의 메세지에 답변하게 합니다.


#### Dialogs

##### `Dialogs/MainDialog.cs`

![5](https://user-images.githubusercontent.com/41438361/87850015-ea4c5500-c927-11ea-9fca-d787e5e15390.JPG)

MainDialog에서는 필요한 Dialog들을 모두 모아 실행시킵니다.

우선 튜토리얼을 실행시키는데 필요한 `TutorialDialog.cs`를 `AddDialog` 함수로 불러와줍니다.

그리고 WaterfallDialog를 설정하여 `InitialStepAsync`, `FinalStepAsync`가 차례대로 실행되게 해줍니다.

![6](https://user-images.githubusercontent.com/41438361/87850064-3f886680-c928-11ea-8939-3f363f3e1b65.JPG)

`InitialStepAsync`에서는 TutorialDialog를 실행시키고, `FinalStepAsync`에서는 Dialog를 종료시킵니다.

##### `Dialogs/TutorialDialog.cs`

![7](https://user-images.githubusercontent.com/41438361/87850098-78284000-c928-11ea-8a36-84d039d7bfc4.JPG)

`TutorialDialog`에서도 `MainDialog`같이 WaterfallDialog를 이용하여 실행시킬 step을 구현합니다.

#### `UserProfile.cs`

![8](https://user-images.githubusercontent.com/41438361/87850137-e0772180-c928-11ea-986d-709a28e34a8f.JPG)

사용자에게 받아야 할 정보를 담을 변수들을 선언합니다.

#### Startup.cs

![9](https://user-images.githubusercontent.com/41438361/87850159-0b617580-c929-11ea-9941-e3b3cb934f94.JPG)

필요한 Dialog들을 추가시키고 Bot을 생성합니다.


## 따라하기

Test2폴더를 참고해주세요.



