# :speech_balloon: Healthee Chatbot Service :runner:
<img src="https://user-images.githubusercontent.com/41438361/88480682-43ecf900-cf92-11ea-875a-79df7aa29d8b.png" width="50%">

## Topic: Hometraining을 도와줄 Azure based chatbot 
홈 트레이닝 챗봇 **Healthee**는 홈 트레이닝을 하려는 분들에게

**운동을 추천**해주고 **운동 기구, 음식에 대한 정보를 제공**하여 운동을 도와드리는 것을 목표하고 있습니다. 

## 시연 링크
[Healthee랑 대화해보기💬](https://htmlpreview.github.io/?https://github.com/yjo5252/chatee/blob/master/Healthee.html)

## 따라하기

1. 시작 전에, 필요한 요소들을 다운받고 세팅해주세요. [영욱님의 스튜디오 Chatbot 설명 영상](https://www.youtube.com/watch?v=g3UR61eOsiA)
2. 프로젝트를 다운받아 주세요. 
3. `Healthee` 폴더에서 `Healthee.sln` 프로젝트를 실행해주세요.
4. 디버그 후 뜨는 윈도우 창에서 
<img src="https://user-images.githubusercontent.com/41438361/88481906-15265100-cf99-11ea-9226-7442e8d26c5c.JPG" width="50%">
에서 형광펜 친 부분을 복사해주세요.
5. Bot Emulator를 켜서 open bot을 누른 후 
<img src="https://user-images.githubusercontent.com/41438361/88481950-4b63d080-cf99-11ea-8b46-4c7bb03e8238.png" width="50%"> 
에 나와있는 것처럼 복사한 값을 넣고 실행시켜 주세요.

## 개발 환경

* [Visual Studio 2019](https://visualstudio.microsoft.com/ko/)
* [Bot Framework v4 SDK Templates for Visual Studio](https://marketplace.visualstudio.com/items?itemName=BotBuilder.botbuilderv4)
* [Bot Emulator](https://github.com/Microsoft/BotFramework-Emulator)
* [Azure Trial](https://azure.microsoft.com/ko-kr/free/)
* Microsoft Azure Sql

*자세한 설명은 [이 영상에서 확인해주세요](https://www.youtube.com/watch?v=g3UR61eOsiA)*

## TEAM: Chatee
<img src="https://user-images.githubusercontent.com/41981471/86508101-52734500-be18-11ea-90e0-92df415e79d2.JPG" width="40%">

## 과제
[가이드 페이지](https://blog.naver.com/formktmkt/221994807603)

### 1. Gate 1

[Gate1 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/Gate1/Gate1.md)

### 2. Gate 2

[Gate2 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/Gate2/Gate2.md)


### 3. Gate 3 

[Gate 3 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/Gate3/Gate3.md)

### 4. 최종 게이트(Gate 4)

:tada:[Gate 4 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/%EC%B5%9C%EC%A2%85%20%EA%B2%8C%EC%9D%B4%ED%8A%B8(Gate%204)/%EC%B5%9C%EC%A2%85%EA%B2%8C%EC%9D%B4%ED%8A%B8%20%EC%84%A4%EB%AA%85.md):tada:

## Healthee가 제공하는 기능

|||
|---------|----|
|<img src="https://user-images.githubusercontent.com/41438361/88453651-d9f62600-cea3-11ea-9df4-3661cd686d8f.JPG" width="75%">|<img src="https://user-images.githubusercontent.com/41438361/88453716-73bdd300-cea4-11ea-8e1f-8b475738f534.JPG" width="75%">|
|<img src="https://user-images.githubusercontent.com/41438361/88455290-301c9680-ceaf-11ea-99fb-c53e5a26ccfc.JPG" width="75%">|<img src="https://user-images.githubusercontent.com/41438361/88455293-314dc380-ceaf-11ea-9a71-5154aa6c7a98.JPG" width="75%">|
|<img src="https://user-images.githubusercontent.com/41438361/88455295-327ef080-ceaf-11ea-8a46-14d59d42ec30.JPG" width="75%">|<img src="https://user-images.githubusercontent.com/41438361/88455296-33b01d80-ceaf-11ea-99d6-c264bae4f440.JPG" width="75%">|

*캐릭터는 사람들이 Healthee와 더 많이 대화를 나누고 운동을 하게끔 동기 부여를 하기 위해 만든 기능입니다! 운동을 하고 많이 기록을 할 수록 캐릭터는 성장합니다.*

## 구조(Final Ver)

### 구조도

<img src="https://user-images.githubusercontent.com/41438361/88481301-83691480-cf95-11ea-9d5f-e275d4cd9247.png" width="60%">

### Flow Chart

![image](https://user-images.githubusercontent.com/41438361/88481739-228f0b80-cf98-11ea-8411-381ff5dc458b.png)

### 구성

* Bots 폴더
  * DialogAndWelcomeBot.cs : 새로운 사용자가 들어왔을 때 인사하는 부분. Echobot을 상속받았다.
  * EchoBot.cs : 기존의 Echobot 변형. 사용자의 입력을 처리한다.

* Dialogs 폴더
  * MainDialog.cs : 가장 상위 Dialog. TutorialDialog를 실행시킨다.
  * CheckUserDialog.cs : 맨 처음에 사용자가 입장했을 때 사용자가 이전에 Healthee를 이용했는지 안했는지 물어보는 Dialog. 만약 예전에 사용해봤으면 사용자의 데이터를 DB에서 가져오고 아니면 튜토리얼을 진행하여 사용자의 정보를 받는다.
  * TutorialDialog.cs : 튜토리얼을 진행하는 Dialog. Waterfall Dialog를 이용하여 질문과 답변이 순차적으로 이루어질 수 있게 구현하였다.
  * ShowFuctionsDialog.cs : 사용자에게 이용가능한 기능을 카드로 보여주는 Dialog.
  * RecommendExerciseDialog.cs : 사용자에게 원하는 부위와 목표에 도움이 되는 운동을 추천해주는 Dialog.
  * RecordDialog.cs : 사용자가 한 운동을 기록할 수 있게 하는 Dialog.
  * RecommendFood.cs : 사용자에게 랜덤으로 음식을 추천해주는 Dialog.
  * RecommendEquipment.cs : 사용자에게 운동 기구를 추천해주는 Dialog.
  * SeeMyCharacterDialog.cs : 사용자가 자신의 캐릭터 상태를 확인할 수 있는 Dialog.
  * SeeMyRecord.cs : 사용자가 자신의 운동 기록을 볼 수 있는 Dialog.

* Resources 폴더
  * CharacterName.json : `TutorialDialog` 에서 캐릭터 이름 설정하라고 하는 Adaptive Card 구현
  * CharacterShow.json : `TutorialDialog` 에서 캐릭터 보여주는 Adaptive Card 구현
  * EquipmentInitial.json : `RecommendEquipmentDialog` 에서 처음 기능 이미지 보여주는 Adaptive Card 구현
  * FoodInitial.json : `RecommendFoodDialog` 에서 처음 기능 이미지 보여주는 Adaptive Card 구현
  * RecordInitial.json : `RecordDialog` 에서 처음 기능 이미지 보여주는 Adaptive Card 구현
  * REFirst.json : `RecommendExercise` 에서 맞춤 추천할건지 물어보는 Adaptive Card 구현
  * REInitial.json : `RecommendExercise` 에서 처음 기능 이미지 보여주는Adaptive Card 구현
  * StartTutorial.json : `TutorialDialog` 에서 처음에 사용자 환영하는 Adaptive Card 구현
  * Welcoming.json : 맨 처음 `DialogAndWelcomBot` 에서 사용자 환영하는 Adaptive Card 구현

* UserProfile.cs : 사용자의 정보를 담을 클래스(사실 이 Healthee에서 필수적인 부분은 아니지만, `TutorialDialog`에서 좀 더 편하게 변수에 접근하기 위해 남겨두었습니다. UserProfile을 쓰는 부분을 UserInfoManager로 바꿔도 무방합니다.)
* UserInfoManager.cs : 사용자의 정보를 관리하기 위한 클래스
* Startup.cs : DialogAndWelcomeBot를 MainDialog로 실행시킨다.
* ModeManager.cs : 봇의 모드(기능 실행 상태)를 관리하기 위한 매니저
* Cards.cs : json형식의 파일을 Adaptive Card로 보여주기 위해 사용되는 매니저
* appsettings.json : 봇의 연결 설정(QnA Maker, MicrosoftApp)


## 코드 설명

### 1. Bots 

실질적인 'Bot'의 역할을 하는 부분. 사용자가 처음 Healthee에 입장했을 때 환영 인사를 하거나, 사용자가 보낸 메세지에 응답을 하기 위한 기능을 구현하는 부분이 존재한다.

#### 1.1 **사용자 환영하기**: `Bots/DialogAndWelcomeBot.cs`

*사용자가 Healthee 봇에 처음 입장했을 때 환영 인사를 보낸다.*

![1](https://user-images.githubusercontent.com/41438361/87849805-76f61380-c926-11ea-9390-325600857f96.JPG)

DialogAndWelcomeBot은 EchoBot을 상속받습니다. EchoBot에 대한 설명은 바로 뒤에서 나옵니다.

**`OnMemebersAddedAsync`** : 사용자를 환영 하는 메소드. \
이 메소드는 봇이 사용자를 처음 인식했을 때, 즉 사용자가 봇에 처음 입장했을 때 실행되는 함수입니다. 말 그대로 사용자가 추가되었을 때 봇이 할 수 있는 동작을 써 놓는 곳입니다.

![41](https://user-images.githubusercontent.com/41438361/88453991-ff386380-cea6-11ea-98fb-79c5791c0740.JPG)

`OnMembersAddedAsync` 함수안에 위의 코드를 넣었습니다.

![1](https://user-images.githubusercontent.com/41438361/88454160-85a17500-cea8-11ea-87f2-6770f005b105.JPG)

`ModeManager.mode`와 `MainDialog.is_running_dialog`를 설정합니다.
처음에 Healthee는 사용자가 이전에 Healthee를 방문했는지 안했는지 확인하기 위해 사용자 확인 작업을 합니다. 이에 대한 자세한 설명은 뒷 부분에 나옵니다.

![2](https://user-images.githubusercontent.com/41438361/88454179-ac5fab80-cea8-11ea-842b-c9c0e9368c35.JPG)

뒤로 카드를 붙일 `attachments`를 생성하고 `reply.Attachments.Add(객체)`로 객체를 붙여줍니다. 위의 코드에서는 'heroCard'와 `Cards.CreateAdaptiveCardAttachment("Welcoming.json")`를 이용하여 객체를 생성하여 `attachment`에 붙였습니다. `CreateAdaptiveCardAttachment` method는 위의 코드에서 `Welcoming.json` 파일을 Adaptive Card로 변환하여 사용자에게 보여질 수 있게 했습니다. 이 메소드는 뒷부분(Cards)에 설명되어 있습니다.

추가로 `turnContext.SendActivityAsync` method를 이용해서 사용자에게 메세지를 보냅니다.

#### 1.2 **사용자 응답에 대응하기**: `Bots/EchoBot.cs`

*사용자가 봇에 메세지를 보내면 응답을 처리하는 부분*

**`OnMessageActivityAsync`** : 사용자 응답에 대응하는 메소드 \
이 함수는 사용자가 봇에 메세지를 보낼때마다 실행이 됩니다. 즉, 사용자가 봇에 메세지를 보낸 것이 인식이 되면 실행이 됩니다. 

아무런 기능이 실행 중이지 않으면 **사용자의 입력이 들어올때마다 현재 봇의 문맥(상황)에 맞는 Dialog를 실행시킬 수 있게 모드를 설정할 수 있습니다**. 
기능이 실행 중이라면 **진행중이던 Dialog를 계속 진행합니다**. 

**`OnMessageActivityAsync`** method 안의 내용을 보면 다음과 같습니다.

![2](https://user-images.githubusercontent.com/41438361/88438311-66bcc780-ce43-11ea-9bb1-e36165d163ab.JPG)

먼저 QnaMaker를 불러옵니다. QnA Maker는 말 그대로 질문에 답변을 해줄 수 있게 하는 API로, 사용자가 질문을 하면 올바른 답을 출력할 수 있게 도와줍니다. QnAMaker를 생성하기 위해 `_configuration[값]`을 통해서 `appsettings.json` 파일에 있는 값들에 접근합니다. 이 `appsettings.json`에서 연결을 세팅하는 방법도 뒤에서 보겠습니다.

![3](https://user-images.githubusercontent.com/41438361/88438359-8227d280-ce43-11ea-953a-ee1cfcdfb473.JPG)

사용자의 입력을 `msg_from_user` 변수로 받고, 공백을 제거하여 처리를 했습니다. 

사용자의 입력을 처리한 후에는 **`MainDialog.is_running_dialog`** 가 0인지 아닌지 판별하여 봇을 실행시킵니다. `MainDialog.is_running_dialog`는 현재 실행중인 Dialog(기능)가 있는지 없는지 확인할 수 있게 하는 변수입니다. 

**`MainDialog.is_running_dialog`가 0이면** 현재 실행중인 Dialog가 없다는 뜻입니다.(현재 실행중인 기능이 없다. 기능 실행 명령을 대기중이다.) 이때는 사용자의 입력을 받아 사용자의 입력 메세지에 특정 기능을 나타내는 **키워드**가 있을 경우 해당 기능을 실행시킬 수 있도록 모드)(`ModeManager.mode`)를 설정합니다. 이 모드에 대한 설명도 뒤에서 보겠습니다. 특히, 사용자 입력에 "목적" 이나 "안녕" 이라는 단어가 포함되면 QnA Maker가 실행될 수 있도록 해놨습니다.

![5](https://user-images.githubusercontent.com/41438361/88438660-34f83080-ce44-11ea-9eda-2a12b6e9906a.JPG)

**`MainDialog.is_running_dialog`가 1이면** 현재 실행중인 Dialog가 있다는 말입니다.(현재 실행중인 기능이 있다. 기능이 진행중이다.) 따라서 이때는 현재 실행중인 기능이 있으므로 기능 수행 도중에 다른 모드로 전환이 되어버리면 안됩니다. 따라서 이때는 모드 전환 부분이 없습니다.

![4](https://user-images.githubusercontent.com/41438361/88438548-f06c9500-ce43-11ea-9f7f-67bb2533aee4.JPG)

마지막으로 `Dialog.RunAsync`는 Dialog를 실행시킬 수 있게 하는 method입니다. 이를`OnMessageActivityAsync` method의 마지막 부분에 위의 코드를 추가하여 Dialog을 실행시킬 수 있도록 합니다.

### 2. Dialogs

Dialog는 봇의 기능과 같은 역할을 합니다. 즉, Dialog들은 내부에 저마다 개별적으로 실행시킬 수 있는 step을 가지고 있는데, 이 step들을 `WaterfallDialog`를 이용하여 순차적으로 실행시켜 특정 기능을 구현하는 것이라고 생각하면 되겠습니다.

![7](https://user-images.githubusercontent.com/41438361/88474208-47b35800-cf5f-11ea-80c6-7ecf0c0a6cab.JPG)

예시) `Dialog A(step 1)->Dialog A(step 2)->Dialog A(step 3)->Dialog B(step 1)->Dialog B(step 2)->Dialog A(step 4)`와 같이 실행시킬 수 있습니다.

Healthee는 Dialog를 이용하여 모드에 따라 다른 기능을 실행할 수 있게, 조건을 확인하여 특정 기능을 수행할지 말지 결정합니다.

#### 2.1 **실행 중인 Dialog 확인 및 필요한 Dialog 추가/실행** : `Dialogs/MainDialog.cs`

*Healthee 구동에 필요한 Dialog들을 모두 실행시키는 관리자 Dialog와 같은 역할을 합니다. 실제 Dialog 끼리의 계층은 없지만 구조상 가장 최상위 Dialog입니다. 여기서는 모드에 따라 다른 Dialog들을 직접 실행시킵니다.*

**실행 중인 Dialog 있는지 확인**

위에 `EchoBot.cs` 에서 현재 실행중인 Dialog가 없을때만(현재 수행중인 기능이 없을 때) 모드를 바뀝니다.

![7](https://user-images.githubusercontent.com/41438361/88439088-3413ce80-ce45-11ea-8a71-557511ee836d.JPG)

`MainDialog.cs`에서는 이 `is_running_dialog`를 통해 현재 실행중인 Dialog가 있는지 없는지 값을 **설정**해줍니다.

**필요한 요소 추가**

![8](https://user-images.githubusercontent.com/41438361/88439370-01b6a100-ce46-11ea-9322-351291627e4b.JPG)

Dialog도 실행시키기 전에 필요한 요소들을 **생성자에서** 세팅해야 합니다. 이 요소들은 

1. Prompt(사용자에게 입력을 촉구하는 부분)
2. 다른 Dialog(Dialog 내에서 직접 다른 Dialog를 실행시킬 때)

과 같습니다. `MainDialog.cs` 는 최상위 Dialog라고 할 수 있는데, 다른 모든 Dialog들을 `MainDialog.cs`안에서 직접 실행시킵니다. 따라서 필요한 Dialog들을 모두 `AddDialog(new DialogName())`으로 불러옵니다. 상위 Dialog에서 하위 Dialog들을 실행시키기 위해서는 꼭 `AddDialog()` 를 통해 하위 Dialog를 추가시켜줘야 합니다.

맨 마지막에 있는 WaterfallDialog는 `{ }`안에 `InitialAsync`, `FinalStepAsync` 처럼 실행시키고 싶은 step(단계)를 추가해 줍니다. WaterfallDialog에 있는 step들은 위에서부터 순서대로 실행됩니다. 마지막으로 `InitialDialogId = nameof(WaterfallDialog)`를 통해 처음에 WaterfallDialog를 실행시키게 합니다.

**Dialog 실행**

![9](https://user-images.githubusercontent.com/41438361/88439527-6c67dc80-ce46-11ea-8a7d-7572cb7d0b39.JPG)

WaterFallDialog의 가장 첫 step인 `InitialDialog`는 위와 같습니다. 보면 `ModeManager.mode`를 참조하여 모드에 따라서 실행시킬 Dialog를 구분하여 실행시킵니다. 또 `MainDialog`를 포함해서 다른 Dialog들을 실행시키게 되는 부분이므로 현재 Dialog가 실행중이라는 `is_running_dialog`를 1로 설정합니다.

위의 `return await stepContext.BeginDialogAsync(nameof(Dialog이름), null, cancellationToken);` 함수는 특정 Dialog에서 바로 다른 Dialog로 이동할 수 있게 해주는 method입니다. 이 method를 통해 다른 Dialog가 실행이 되고 다른 Dialog가 종료된다면 이 Dialog의 다음 step이 실행되게 됩니다.

예를 들어 DialogA에 step1, step2, step3가 있고 step2에서 DialogB를 실행시켰다면 DialogB가 종료되었을 때 다시 DialogA의 step2로 돌아오는 것이 아닌 step3로 돌아가게 됩니다.

![10](https://user-images.githubusercontent.com/41438361/88439661-c2d51b00-ce46-11ea-9080-2bf228f86ce4.JPG)

마지막 step인 FinalstepAsync에서는 `is_running_dialog`를 0으로 설정하여 Dialog가 끝났음을 알립니다.

모든 Dialog에는 위처럼 `return await stepContext.EndDialogAsync(null, cancellationToken);` 가 포함이 되어야 합니다. 이 method는 Dialog를 종료시키는 역할을 합니다. 만약 WaterfallDialog의 중간 step에서 `EndDialogAsync` method가 실행되었다면 뒤에 얼마나 많은 step이 남았는지는  바로 해당 Dialog는 종료됩니다.

#### 2.2 **사용자가 튜토리얼을 진행했는지 확인**: `Dialogs/CheckUserDialog.cs`

*`CherkUserDialog`에서는 사용자가 처음 봇에 들어왔을 때 이전에 Healthee와 대화를 나눠봤는지, 아닌지 판단합니다. 만약 대화를 나눠봤다면 이전에 저장된 사용자의 데이터를 Azure Sql Database에서 가져오고 기능 카드를 출력해주고, 아닐 경우 사용자의 정보를 받는 `TutorialDialog`를 실행시킵니다.*

![11](https://user-images.githubusercontent.com/41438361/88439858-51e23300-ce47-11ea-8da9-8e4ca5752cbc.JPG)

(윗 부분은 `DialogAndWelocomeBot.cs`의 `OnMembersAddedAsync` method에서 선언된 부분입니다.) Healthee는 처음에 모드를 InitialCheckUser 모드로 설정하여 `CheckUserDialog.cs`가 실행될 수 있도록 했습니다. 

정리하면, 설정된 모드로 1. `EchoBot.cs`에서 `Dialog.RunAsync()` method가 실행되면서 현재 수행중이던 Dialog, 혹은 새로 Dialog를 시작하는데 이때 새로 기능을 실행시킬 경우(`MainDialog.is_running_dialog`가 0일 경우) 먼저 2. `MainDialog.cs`가 실행되고, 3. 모드 확인 후 모드에 맞는 Dialog가 실행될 수 있도록 되는 구조입니다. 처음에 모드를 InitialCheckUser 모드로 설정하였으므로 사용자가 입장하고 나서 `MainDialog.cs` -> `CheckUserDialog.cs` 순으로 실행되는 것입니다.

**필요한 요소 추가**

![12](https://user-images.githubusercontent.com/41438361/88440318-99b58a00-ce48-11ea-8d4a-bc86f4f2eed3.JPG)

`MainDialog.cs`와 마찬가지로 생성자 부분에 필요한 Dialog를 추가하고 WaterfallDialog를 구현합니다. 이 Dialog에서는 `TutorialDialog.cs`와 `ShowFucntionDialog`를 직접 실행시키므로 `AddDialog()` method를 이용해서 추가해줍니다.

`TextPrompt`, `ChoicePrompt`같은 것은 step에서 다음 step으로 넘어갈 때 사용자가 입력해야 하는 포맷이 정해져 있을 때(`TextPrompt`일 경우 사용자는 다음 step으로 넘어가기 위해 텍스트를 입력해야 하고 `ChoicePrompt`일 경우 나열되는 선택지 중에 하나를 선택해야 합니다.) 필요한 프롬프트인데, WaterfallDialog의 각 step에서 사용할 수 있습니다. 이것들도 하위 Dialog와 마찬가지로 `AddDialog()` method를 이용해서 추가시켜야 정상적으로 이용할 수 있습니다.

![13](https://user-images.githubusercontent.com/41438361/88440520-24968480-ce49-11ea-9e8d-361e517f658a.JPG)

예를 들어, 이 Dialog에서 가장 처음에 실행되는 `AskVisitedAsync` 단계에서는 다음 단계로 넘어가기 위해 `ChoicePrompt`를 이용하여 사용자에게 "응", "아니" 중 하나를 선택하도록 만들었습니다. 만약 사용자가 이 외의 값을 입력할 경우 둘 중 하나의 값을 무조건 입력할 때까지 이 step이 반복됩니다.

![8](https://user-images.githubusercontent.com/41438361/88474335-58180280-cf60-11ea-99d1-ed7b27ee36da.JPG)

화면에는 위와 같이 나오게 됩니다.

`AddDialog`로 마지막에 추가시킨 `WaterfallDialog`는 `{}`안에 있는 step들을 순차적으로 실행시켜주는 역할을 합니다. 맨 마지막에 `InitialDialogId = nameof(WaterfallDialog)`때문에 이 `CheckUserDialog.cs`를 실행시키면 가장 먼저 `WaterfallDialog`가 실행되고 가장 첫 step인 `AskVisitedAsync`가 실행되게 됩니다.


**Dialog 실행**

![14](https://user-images.githubusercontent.com/41438361/88440606-70e1c480-ce49-11ea-928f-cbcce3739287.JPG)

이 Dialog에서 두번째로 실행되는 step인 `AskNameAsync`에서는 `((FoundChoice)stepContext.Result).Value.Trim()`를 이용해서 이전 단계에서 사용자가 입력한(선택한) 값을 가져온 후 공백을 없애줍니다. 이처럼 항상 사용자에게 prompt를 통해 입력값을 받으면 다음 step에서 받아올 수 있습니다.

만약 "응"을 선택했을 경우, `TextPrompt`를 이용해서 사용자에게 이름을 입력하도록 만들었고, "아니"를 선택했을 경우 `stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);`를 이용하여 `TutorialDialog.cs`를 실행시킵니다. `BeginDialogAsync` method는 특정 Dialog를 바로 실행시킬 수 있게 하는 함수입니다.

`stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 1;`은 WaterFallDialog에서 특정 step을 실행시키고 싶을 때 이용할 수 있습니다. 만약 마지막에 `+1`이 아닌 `0`을 했다면 바로 다음 단계가 실행됩니다. `-1`을 했다면 현재 단계가 다시 실행될 것입니다. Healthee는 `+1`를 하여 WaterfallDialog에서 한 step을 건너뛸 수 있도록 했습니다. 만약 이렇게 설정하지 않는다면, WaterfallDialog에 있는 step들은 무조건 차례대로 실행되기 때문에 바로 다음 step이 실행됩니다. 이 코드에서는 `TutorialDialog.cs`를 마치고 다시 이 Dialog로 돌아왔을 때 한 step을 건너뛰겠다라는 말이 됩니다.

![9](https://user-images.githubusercontent.com/41438361/88474444-75010580-cf61-11ea-9084-03bed62a855a.JPG)

즉 위의 그림같이 됩니다.

![15](https://user-images.githubusercontent.com/41438361/88440862-30367b00-ce4a-11ea-96db-dfae67146f51.JPG)

**Azure sql 데이터베이스 이용**

다음 step인 `CheckValidUserAsync` 에서는 사용자의 이름을 가지고 Azure sql 에 있는 데이터베이스를 검색해 사용자 이름 정보가 `UserInfo` 테이블에 있는지 검사합니다. Azure sql과 Bot을 연결하는 부분, 쿼리문을 이용하는 방법도 후에 추가로 밑에 설명을 달아놓겠습니다.

![16](https://user-images.githubusercontent.com/41438361/88441063-dda98e80-ce4a-11ea-9b9a-b1cc20bccfe1.JPG)

쿼리들을 이용하여 해당 username이 데이터베이스에 없으면 위와 같이 `TutorialDialog.cs`를 실행시킵니다.

![17](https://user-images.githubusercontent.com/41438361/88441088-f619a900-ce4a-11ea-8f6d-4e298bdf0ea3.JPG)

만약 데이터베이스에 있다면 위와 같이 `UserInfoManager`에 정보들을 저장하고 `ShowFunctionDialog`를 실행시킵니다. `UserInfoManager`에 관해서도 뒷부분에 설명을 달아놨습니다.

#### 2.3 `Dialogs/TutorialDialog.cs`

**필요한 요소 추가 및 Dialog의 WaterfallDialog 구성**

![18](https://user-images.githubusercontent.com/41438361/88441179-3f69f880-ce4b-11ea-860a-f0e45aa59262.JPG)

`TutorialDialog.cs`에서 실행시키는 Step들은 위와 같습니다. step의 주석에서도 볼 수 있듯이 `TutorialDialog`에서는 사용자 정보를 받아 저장하고, 데이터베이스에 넣는 역할을 합니다.

`AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), WeightPromptValidatorAsync));`는 `NumberPrompt`를 실행시킬 때 `WeightPromptValidatorAsync`를 유효성 검사기로 사용하겠다는 뜻입니다.

![20](https://user-images.githubusercontent.com/41438361/88441359-d2a32e00-ce4b-11ea-8e12-ba5e43ef375e.JPG)

예를 들어, 이 Dialog에서 실행되는 `PreWeightStepAsync` 단계에서는 사용자에게 현재 체중을 `NumberPrompt<int>`를 이용하여 int 값으로 입력하게 만듭니다. 사용자가 숫자를 입력하면, 

![21](https://user-images.githubusercontent.com/41438361/88441444-1eee6e00-ce4c-11ea-940b-37233c3b764a.JPG)

위의 `PreWeightStepAsync`가 실행되어 입력한 값을 검사합니다. 만약 입력한 값이 `>0`조건을 만족시킬 경우 다음 step으로 정상적으로 넘어가고, 만약 조건을 만족하지 않았을 경우 `RetryPrompt`가 출력되며 사용자에게 다시 입력할 것을 봇이 얘기하게 됩니다. 이처럼 특정 prompt에서 조건을 확인할 수 있는 함수를 생성할 수 있습니다.

![10](https://user-images.githubusercontent.com/41438361/88474704-31f46180-cf64-11ea-89ab-bb043efcfe04.JPG)

화면 상으로는 위와 같이 나오게 됩니다.

**Dialog 실행**

![19](https://user-images.githubusercontent.com/41438361/88441267-83f59400-ce4b-11ea-8e80-782201c8c8d8.JPG)

후에 실행되는 `AcknowledgementStepAsync` 단계에서는 위와 같이 INSERT 쿼리문을 이용하여 받은 사용자 정보들을 조합하여 데이터베이스에 값을 삽입합니다.

![11](https://user-images.githubusercontent.com/41438361/88474743-839cec00-cf64-11ea-92fd-aeef11083937.JPG)

데이터 베이스에 값을 삽입한 후에는 위와 같이 모드를 바꾸고 `ShowFunctionsDialog` 를 실행시켜 Tutorial이 끝나자마자 기능 카드를 보여주는 Dialog를 실행시킬 수 있도록 했습니다. 뒤에서도 언급하겠지만, 모든 Healthee 기능 Dialog들 마지막에 이렇게 기능 보여주는 부분을 실행시켜 기능들이 끝나고 나서 기능들을 보여줄 수 있도록 했습니다. `Thread.Sleep(3000)` 을 한 이유는 튜토리얼이 끝나자마자 바로 기능 카드를 보여주면 이전의 메세지가 위로 바로 올라가버리기 때문에 사용자에게 메세지를 확인할 수 있는 시간을 주기 위해서입니다.

#### 2.4 **Healthee의 기능 보여주기**: `Dialogs/ShowFunctionsDialog.cs`

*`ShowFunctionDialog`에서는 Healthee가 제공하는 기능들을 카드로 보여줍니다.*

**필요한 요소 추가 및 Dialog의 WaterfallDialog 구성**

![12](https://user-images.githubusercontent.com/41438361/88474819-3ec58500-cf65-11ea-9a35-701b6bae18a0.JPG)

`ShowFunctionsDialog`에서 실행시키는 step들은 위와 같습니다.

**Dialog 실행**

![22](https://user-images.githubusercontent.com/41438361/88441622-93c1a800-ce4c-11ea-9705-65b52bb37ea6.JPG)

카드를 보여주는 step인 `ShowCardStepAsync`에서는 위와 같이 `attachments`에 `HeroCard` 객체를 생성하여 붙입니다. 객체는 `HeroCard` 뿐만 아니라 다양한 객체들이 될 수 있습니다. `HeroCard`는 카드에 이미지, 글 등 다양한 요소를 넣어 보여줄 수 있는 객체입니다. 이런 카드들에 대한 추가 예시는 [Microsoft 공식문서](https://docs.microsoft.com/ko-kr/azure/bot-service/bot-service-design-user-experience?view=azure-bot-service-4.0)에서 확인할 수 있습니다. 

추가로 `reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;`를 이용하여 `attachments`에 붙여진 객체들을 회전식으로 보여줍니다.

`HeroCard` 안에는 다양한 필드를 이용하여 다양한 카드를 만들 수 있습니다. 여기서의 `Buttons-CardAction - ActionTypes.ImBack`은 사용자가 `Button`을 누르면 `value`의 값을 봇한테 전송하는 것을 가능하게 하는 ActionType입니다.

봇에서는 다음과 같이 보입니다.

![1](https://user-images.githubusercontent.com/41438361/88474964-87ca0900-cf66-11ea-88a5-d3ccd8877120.png)


오른쪽에 마우스를 갖다대면 위의 사진처럼 화살표가 나와 클릭하면 카드를 이동시키며 볼 수 있습니다. 만약 기능 카드들에 있는 각 버튼을 클릭하면 다음과 같이 나옵니다.

![image](https://user-images.githubusercontent.com/41438361/88474973-b34cf380-cf66-11ea-981a-ba6082bf9ff5.png)

#### 2.5 `Dialogs/RecommendExerciseDialog.cs`

*`RecommendExerciseDialog`에서는 사용자에게 운동을 추천해줍니다. 이전에 저장된 사용자 정보를 바탕으로 맞춤운동을 추천해줄 수도 있고, 새로 사용자의 입력을 받아 그 결과에 맞는 운동을 추천해줄 수도 있습니다. 운동을 추천한 후에는 사용자가 운동을 했는지 안했는지 확인하여 데이터베이스에 운동 기록 데이터를 업데이트 할 수도 있습니다.*

**필요한 요소 추가 및 Dialog의 WaterfallDialog 구성**

![24](https://user-images.githubusercontent.com/41438361/88441859-6aede280-ce4d-11ea-9fa7-139b55513a0a.JPG)

`RecommendExerciseDialog`에서 실행되는 step들은 위와 같습니다. 

* `AskUserAsync`에서는 사용자에게 맞춤운동을 추천해줄지 물어봅니다. 
* `CheckAnswerAsync`에서는 사용자가 이전에 답한 값을 확인합니다. 사용자가 추천해달라고 하면 바로 운동을 보여주는 `ShowExerciseAsync`단계로 넘어가고, 아니라면 사용자에게 정보를 묻는 `SelectAreaAsync`로 넘어갑니다. 이렇게 추가적인 단계를 둔 이유는 특정 단계에서 사용자에게 `prompt`로 입력을 받으면 다음 단계로 무넘어가기 때문입니다. 
* `SelectAreaAsync`에서는 사용자가 운동할 부위를 묻습니다.
* `SelectKindAsync`에서는 사용자가 운동할 운동의 종류를 묻습니다.
* `SelectLevelAsync`에서는 사용자 운동할 운동의 난이도를 묻습니다.
* `ShowExerciseAsync`에서는 정보를 바탕으로 DB에 접속하여 운동 정보를 받아와 사용자에게 운동을 추천해줍니다.
* `DidExerciseAsync`에서는 사용자가 추천한 운동을 했는지 묻습니다.
* `ShowResultStepAsync`에서는 사용자가 운동을 했으면 DB에 접속하여 운동 기록 정보를 업데이트 하고, 운동을 안했다면 아무런 작업도 하지 않습니다. 기능 카드를 보여주는 다이얼로그를 실행시키며 마무리합니다.
* `EndAsync`에서는 다이얼로그를 종료시킵니다.

**Dialog 실행**

마찬가지로 prompt와 db query, herocard 및 adaptiveCard를 이용하여 기능들을 구현했습니다. `AdaptiveCard`는 사용자가 카드의 구성을 커스터마이징 하기 쉬운 형식의 카드입니다. 자세한 설명은 뒤에서 다시 다루겠습니다.

결과는 다음과 같이 나옵니다.

![image](https://user-images.githubusercontent.com/41438361/88475043-5dc51680-cf67-11ea-9127-963419aeb0b2.png)
![image](https://user-images.githubusercontent.com/41438361/88475054-75040400-cf67-11ea-965e-96b42110981b.png)
![image](https://user-images.githubusercontent.com/41438361/88475068-8e0cb500-cf67-11ea-9508-00df734e0904.png)
![image](https://user-images.githubusercontent.com/41438361/88475077-a250b200-cf67-11ea-86d4-de3e33d0328c.png)

#### 2.6 **운동 기록**: `Dialogs/RecordDialog.cs`

* `RecordDialog`는 사용자가 운동을 기록할 수 있게 하는 기능을 구현했습니다. 사용자가 운동한 부뷔와 시간을 입력하면 운동 기록을 DB에 업데이트합니다.*

* `InitialRecordAsync`에서는 운동을 기록하겠다는 `AdaptiveCard`를 보여주고 필요한 변수들의 값을 초기화해줍니다.
* `CheckAreaAsync`에서는 사용자가 운동한 부위를 묻습니다.
* `CheckTimeAsync`에서는 사용자가 운동한 시간을 묻습니다.
* `RecordAsync`에서는 사용자가 운동한 기록을 DB에 업데이트 시키고 기능 카드를 보여주는 다이얼로그를 실행시키고 마무리합니다.
* `EndAsync`에서는 다이얼로그를 종료시킵니다.

**필요한 요소 추가 및 Dialog의 WaterfallDialog 구성**

![25](https://user-images.githubusercontent.com/41438361/88442024-ed76a200-ce4d-11ea-9adb-7445a26db6ea.JPG)

`RecordDialog`에서 실행되는 step들은 위와 같습니다. 

**Dialog 실행**

봇에서는 다음과 같이 나옵니다.

![image](https://user-images.githubusercontent.com/41438361/88475534-b0083680-cf6b-11ea-85f4-886dcccf0055.png)


#### 2.7 `Dialogs/RecommendFood.cs`

*`RecommendFood`에서는 랜덤으로 사용자에게 음식을 추천해줍니다. 추천할때는 음식의 이름, 사진이 있을경우 사진, 설명까지 함께 `Herocard`로 보여줍니다.*

**필요한 요소 추가 및 Dialog의 WaterfallDialog 구성**

![26](https://user-images.githubusercontent.com/41438361/88442254-c9679080-ce4e-11ea-8e12-93f57483a3da.JPG)

`RecommendFood`에서 실행되는 step들은 위와 같습니다. 

* `InitialAsync`에서는 음식 랜덤 추천이라는 카드를 보여줍니다.
* `RecommendResultAsync`에서는 음식 추천 카드를 보여줍니다. 기능 카드를 보여주는 Dialog를 실행시키고 마무리합니다.
* `EndAsync`에서는 다이얼로그를 종료시킵니다.

**Dialog 실행**

봇에서는 다음과 같이 나옵니다.

![image](https://user-images.githubusercontent.com/41438361/88475586-68ce7580-cf6c-11ea-84db-79c8eb3d60ba.png)

#### 2.8 `Dialogs/RecommendEquipment.cs`

*`RecommendEquipment`에서는 사용자가 선택한 정보를 토대로 운동기구를 추천해줍니다. 운동 기구를 추천해줄때는 판매처, 영상까지 함께 `HeroCard`로 보여줍니다.*

**필요한 요소 추가 및 Dialog의 WaterfallDialog 구성**

![27](https://user-images.githubusercontent.com/41438361/88442288-e9974f80-ce4e-11ea-9e88-1efa1d093ef1.JPG)

`RecommendEquipment`에서 실행되는 step들은 위와 같습니다. 

* `InitialAsync`에서는 운동 기구 추천이라는 카드를 보여줍니다.
* `SelectAreaAsync`에서는 어떤 부위를 운동하는 기구를 추천해줄건지 물어봅니다.
* `RecommendResultAsync`에서는 운동 기구를 추천해줍니다. 후에 기능 카드를 보여주는 Dialog를 실행시키고 마무리합니다.
* `EndAsync`에서는 다이얼로그를 종료시킵니다.

**Dialog 실행**

봇에서는 다음과 같이 나옵니다.

![image](https://user-images.githubusercontent.com/41438361/88475657-ff029b80-cf6c-11ea-8e6a-ae346532a287.png)
![image](https://user-images.githubusercontent.com/41438361/88475668-0f1a7b00-cf6d-11ea-9949-4acc77080e95.png)

#### 2.9 `Dialogs/SeeMyCharacterDialog.cs`

*`SeeMyCharacterDialog`에서는 내 캐릭터의 상태를 보여줍니다. 이 dialog에서는 사용자의 정보를 조회하여 사용자의 운동 기록 횟수를 통해 캐릭터가 어떤 상태인지 판단합니다. 캐릭터의 상태에 따라 다른 이미지와 문구를 사용자에게 `HeroCard`로 보여줍니다.*

**필요한 요소 추가 및 Dialog의 WaterfallDialog 구성**

![28](https://user-images.githubusercontent.com/41438361/88442433-72ae8680-ce4f-11ea-97cb-7734e92c6ec0.JPG)

`SeeMyCharacterDialog`에서 실행되는 step들은 위와 같습니다. 

* `ShowCharacterAsync`에서는 현재 운동 기록 횟수를 확인하고 캐릭터를 보여줍니다.
* `EndAsync`에서는 다이얼로그를 종료시킵니다.

**Dialog 실행**

봇에서는 다음과 같이 나옵니다.

![image](https://user-images.githubusercontent.com/41438361/88475734-a384dd80-cf6d-11ea-8efd-744df6196897.png)

운동을 기록하는 횟수가 늘어날 수록 아래와 같이 변화합니다. 더 많은 상태가 있습니다.

![image](https://user-images.githubusercontent.com/41438361/88475763-d333e580-cf6d-11ea-96dd-14729c648b06.png)
![image](https://user-images.githubusercontent.com/41438361/88475774-f2cb0e00-cf6d-11ea-8402-9c792b2d5676.png)
![image](https://user-images.githubusercontent.com/41438361/88475813-36be1300-cf6e-11ea-8f34-f8083128744d.png)

#### 2.10 `Dialogs/SeeMyRecord.cs`

*`SeeMyRecord`에서는 DB에서 사용자의 정보를 조회하여 부위별 운동 시간을 `ReceiptCard`로 보여줍니다.*

**필요한 요소 추가 및 Dialog의 WaterfallDialog 구성**

![29](https://user-images.githubusercontent.com/41438361/88442505-b903e580-ce4f-11ea-956c-e9c46a092283.JPG)

`Dialogs/SeeMyCharacterDialog`에서 실행되는 step들은 위와 같습니다.

* `ShowRecordAsync`에서는 DB에서 사용자의 운동기록을 조회한 후 카드로 보여줍니다.
* `EndAsync`에서는 다이얼로그를 종료시킵니다.

**Dialog 실행**

봇에서는 다음과 같이 나옵니다.

![30](https://user-images.githubusercontent.com/41438361/88442566-f49eaf80-ce4f-11ea-8f56-13720ce6adfc.JPG)


### 3. Resources

여기에 있는 json파일들은 모두 `AdaptiveCard`로 변환될 수 있는 양식입니다.

직접 일일이 만들지 않아도 되는데, [AdaptiveCard 쉽게 만들기](https://adaptivecards.io/designer/)에서 쉽게 UI로 만들 수 있습니다.

![image](https://user-images.githubusercontent.com/41438361/88477460-252f3800-cf7b-11ea-9cd6-3b602c012917.png)

위의 사진처럼 원하는 대로 Adaptive Card를 편집할 수 있습니다. 아래쪽에 json 형식에 사용 가능한 text가 나오므로 이것을 그대로 복사하여 사용하면 됩니다.

`Resources`폴더의 `CharacterShow.json`을 예시로 보겠습니다.

```json
{
  "type": "AdaptiveCard",
  "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
  "version": "1.2",
  "body": [
    {
      "type": "Container",
      "items": [
        {
          "type": "RichTextBlock",
          "inlines": [
            {
              "type": "TextRun",
              "text": "가취가욥~~!💨💨",
              "color": "Good",
              "italic": true
            }
          ],
          "horizontalAlignment": "Center"
        },
        {
          "type": "Image",
          "url": "https://t1.daumcdn.net/section/oc/b91f421cab1a46dd96a845f0c55b7f91",
          "size": "Stretch"
        },
        {
          "type": "Container",
          "items": [
            {
              "type": "RichTextBlock",
              "inlines": [
                {
                  "type": "TextRun",
                  "text": "여러분과 함께 운동할 캐릭터에요.💪 \n\n지금은 "
                },
                {
                  "type": "TextRun",
                  "text": "비실비실",
                  "size": "medium",
                  "weight": "bolder"
                },
                {
                  "type": "TextRun",
                  "text": "하지만 같이 운동하면서 건강해질거에요!"
                }
              ]
            }
          ]
        }
      ]
    }
  ],
  "actions": [
    {
      "type": "Action.Submit",
      "title": "좋아!👍",
      "data": "좋아!"
    }
  ]
}
```

위처럼 json 파일을 만들고 

![image](https://user-images.githubusercontent.com/41438361/88477695-1cd7fc80-cf7d-11ea-8a5e-9d635e824323.png)

`Cards.cs`의 `CreateAdaptiveCardAttachment` method를 이용하여 위처럼 코드를 사용하면

![image](https://user-images.githubusercontent.com/41438361/88477730-5ad52080-cf7d-11ea-9abf-1e1c03c152f1.png)

위처럼 봇에서 확인할 수 있습니다.

### 4.`Cards.cs`

![31](https://user-images.githubusercontent.com/41438361/88442694-6f67ca80-ce50-11ea-8046-754524a66c84.JPG)

`CreateAdaptiveCardAttachment` method를 이용해서 `cardName`을 "~.json" 형식으로 적어주면 해당 json 파일을 adaptive card로 변환할 수 있습니다.

### 5. `ModeManager.cs`

![32](https://user-images.githubusercontent.com/41438361/88442761-af2eb200-ce50-11ea-91ee-eea8ff0c3a85.JPG)

`ModeManager` 에서는 모드(Healthee가 지원하는 기능별로 존재)를 선언합니다. 이 모드를 다른 코드에서 바꿔가며 모드를 조작해줍니다.

* InitialCheckUser : 처음에 사용자가 들어왔을 때 이전에 사용자가 Healthee와 대화해봤는지 체크해야 할 때
* ShowFunction : 기능 카드를 보여줄 때
* Tutorial : 튜토리얼 모드
* Record : 사용자가 운동을 기록할 때
* RecommendFood : 음식을 추천할 때
* RecommendEquipment : 운동 기구 추천할 때
* CheckCharacterState : 캐릭터 상태 확인할 때
* SeeMyRecord : 내 기록 확인할 때


### 6. `UserInfoManager.cs`

![34](https://user-images.githubusercontent.com/41438361/88442864-09c80e00-ce51-11ea-8fc6-ccebb59e1308.JPG)

`UserInfoManager`에서는 사용자 정보를 저장하기 위한 변수를 선언합니다.

### 7. `Startup.cs`

![35](https://user-images.githubusercontent.com/41438361/88442923-354af880-ce51-11ea-96e6-5aa2c17ff0e2.JPG)

`Startup.cs`에서는 필요한 설정들과 Dialog를 추가하여 Bot을 생성합니다. 여기서 `services.AddTransient<IBot, DialogAndWelcomeBot<MainDialog>>();` 때문에 기본적으로 `MainDialog`가 실행이 되는 것입니다.

### 8. `appsettings.json`

![36](https://user-images.githubusercontent.com/41438361/88443003-91158180-ce51-11ea-8763-883c454e2355.JPG)

`appsettings.json`에서는 위와 같이 필요한 key와 value를 설정합니다.

#### MicrosoftApp 연결하는 방법(Azure Portal의 웹 앱과 연결-C# 프로젝트에서 게시했을 때 반영될 수 있게)

* `MicrosoftAppId`, `MicrosoftAppPassword`

![15](https://user-images.githubusercontent.com/41438361/88477910-ea2f0380-cf7e-11ea-9c1e-b197ac45d56c.JPG)

Azure Portal에 접속하여 로그인하고 본인의 리소스 그룹으로 들어가면 위와 같은 화면이 나옵니다. 저기서 형관펜으로 친 웹 앱 봇을 클릭합니다.

![16](https://user-images.githubusercontent.com/41438361/88477944-4a25aa00-cf7f-11ea-8e7e-5260a94144fe.JPG)

왼쪽의 앱 서비스 설정 메뉴의 구성 탭에 들어가면 형광펜으로 친 것처럼 MicrosoftAppId, MicrosoftAppPassword를 확인할 수 있습니다. 여기에 있는 값을 `appsettings.json`에 넣으면 됩니다.

#### QnA Maker 연결하는 방법

우선 QnA Maker가 없다면 QnA Maker를 만듭니다. 만드는 방법은 [여기](https://docs.microsoft.com/ko-kr/azure/cognitive-services/qnamaker/quickstarts/create-publish-knowledge-base)에 나온대로 하시면 됩니다.

그 다음 QnA Maker portal의 윗부분에 My knowledge bases라는 메뉴를 누르면 

![17](https://user-images.githubusercontent.com/41438361/88478034-fa93ae00-cf7f-11ea-9d8c-584dcc5ab556.JPG)

위처럼 만든 QnA KB를 확인할 수 있습니다. View Code를 누르면

![image](https://user-images.githubusercontent.com/41438361/88478121-b3f28380-cf80-11ea-8b16-1d0c9af4dd79.png)

위와 같은 창이 뜹니다. 여기서 표시해 둔 값들을 복사하여 붙이면 됩니다.

*QnA Maker를 봇에 연결할 때, 만약 기존에 연결하려는 봇이 기본 EchoBot일 경우 appsettings.json에 위의 값들을 설정해도 잘 되지 않을 수 있습니다. 이럴때는 QnA Maker 포탈에서 QnA Maker를 만든 후 거기서 웹 앱 봇을 만들고 코드에서 연결하면 잘 됩니다.*


## 주의 사항

### 모듈

추가적으로 모듈을 설치할 필요는 없습니다. 현재 Healthee에서 사용하고 있는 NuGet Package는 다음과 같습니다.

* AdaptiveCards
* AutoMapper
* EntityFramework
* Microsoft.AspNetCore.Mvc.NewtonsoftJson
* Microsoft.Bot.Builder
* Microsoft.Bot.Builder.AI.QnA
* Microsoft.Bot.Builder.Dialogs
* Microsoft.Bot.Builder.Integration.AspNet.Core
* Microsoft.Recognizers.Text
* Microsoft.Recognizers.Text.DateTime
* Microsoft.Recognizers.Text.Number

### 데이터베이스 관련(방화벽)

봇을 C# project로 다운받아 로컬에서 실행시켜 emulator로 확인할 때, 간혹 쿼리문과 연결 설정이 올바르게 되어 있는데도 연결 오류가 뜨는 경우가 있었습니다. 

이 경우 방화벽 문제였는데, 

![1](https://user-images.githubusercontent.com/41438361/88452230-fc824200-ce97-11ea-989c-72436e7fd200.PNG)

Azure Portal > Sql Server > 해당 데이터베이스 > 쿼리 편집기 에서 위와 같이 로그인을 할 때 IP address와 함께 방화벽 문제로 연결이 되지 않는다는 메세지가 떴습니다.

현재 Healthee의 sql 데이터베이스 설정은 아래와 같습니다.

![37](https://user-images.githubusercontent.com/41438361/88443331-d25a6100-ce52-11ea-967c-0156d9faf43c.JPG)

설정을 위와 같이 해놓았지만 항상 새로운 IP에서 접속하기 위해서는 해당 IP주소를 portal을 이용해서 추가해 주어야 했습니다.

![38](https://user-images.githubusercontent.com/41438361/88443389-18afc000-ce53-11ea-9183-fd2077a7355d.JPG)

만약 Chatbot을 구동중 DB 연결 오류가 계속 뜰 시에는 Azure Portal > 해당 **sql server**(데이터베이스가 아닙니다) > 보안 > 방화벽 및 가상 네트워크에서 위에 있는 클라이언트 IP 주소를 시작 IP,  종료 IP에 넣어주고 추가한다음, 위에 있는 저장 버튼을 누르고 다시 접속해주세요. (규칙이름은 아무 값이나 넣어도 됩니다.) 이렇게 해도 안 될 경우 이 *데이터베이스 관련* 부분의 처음 사진에 있는 형광펜으로 친 IP 주소를 추가해주세요.

### 특정 기능이 끝나고 왜 바로 기능카드를 보여주지 않는지

주의 사항까지는 아니지만, 설명해야 하는 부분인 것 같아서 추가하였습니다.

Healthee는 코드에서 `Thread.Sleep(3000);`을 이용하여 특정 기능이 끝나면 바로 기능 카드들을 보여주는 것이 아니라 일정 시간이 지나면 기능카드를 보여줍니다.

그 이유는 사용자가 결과를 확인해야 하는 메세지를 봇이 보내고, 바로 기능 카드를 보여주면 결과 메세지가 바로 위로 올라가버려 사용자가 확인하기 힘들어지기 때문에 사용자가 충분히 결과를 확인할 수 있는 시간을 주기 위해서입니다.

## 데이터베이스

### 데이터베이스 설명

Healthee는 Azure Sql 데이터베이스를 이용하였습니다.

![image](https://user-images.githubusercontent.com/41438361/88478328-7858b900-cf82-11ea-8a79-f4bccff6720c.png)

* AvatarInfo : 캐릭터 정보 담기 위한 테이블

![image](https://user-images.githubusercontent.com/41438361/88478355-9cb49580-cf82-11ea-8004-d8085a5ceaba.png)

* Equipment : 운동 기구 정보 담기 위한 테이블

![image](https://user-images.githubusercontent.com/41438361/88478382-b7870a00-cf82-11ea-9a7d-c2e531e018ac.png)

* ExerciseRecord : 사용자의 운동 기록 담기 위한 테이블

![image](https://user-images.githubusercontent.com/41438361/88478392-ce2d6100-cf82-11ea-859d-760d95fc271d.png)

* Food : 음식 정보 담기 위한 테이블

![image](https://user-images.githubusercontent.com/41438361/88478402-ebfac600-cf82-11ea-90e0-5ecd61b162b0.png)

* Sports : 운동 정보 담기 위한 테이블

![image](https://user-images.githubusercontent.com/41438361/88478410-07fe6780-cf83-11ea-9e39-2e3f9abb992c.png)

* UserInfo : 사용자의 정보를 담기 위한 테이블

### AZURE PORTAL에서 데이터베이스 쿼리문 이용하여 데이터 조작하기

![2](https://user-images.githubusercontent.com/41438361/88452797-a794fa80-ce9c-11ea-805b-cc7129acb827.PNG)

AZURE Portal에 접속한 후, 리소스 그룹에서 SQL 데이터베이스(혹은 SQL 서버 > SQL 데이터베이스)에 접속합니다.

형광펜을 친 곳에 암호를 기입하고 확인을 눌러 쿼리 편집기를 시작합니다.

![3](https://user-images.githubusercontent.com/41438361/88452857-078ba100-ce9d-11ea-88b1-86c34ddb516b.PNG)

쿼리 편집기로 넘어가면, 위와 같이 왼쪽에서는 테이블들과 필드, 그리고 오른쪽에는 직접 쿼리문을 쳐서 실행시킬 수 있는 편집기가 나옵니다.

0. 테이블 생성 CREATE

```sql 
CREATE TABLE [dbo].[Persons] (
    [FirstName] VARCHAR (255) NULL,
    [LastName]  VARCHAR (255) NULL,
    [Age]       INT           NULL,
    CHECK (len([FirstName])>(3) AND len([FirstName])<(50)),
    CHECK ([Age]<=(130)),
    CHECK (len([FirstName])>(3) AND len([FirstName])<(50))
);
```
예를 들어 `Persons` 라는 테이블을 만들고 싶다면 위처럼 입력하면 새로운 `Persons` 테이블이 만들어집니다.

1. 데이터 조회 SELECT

```sql
SELECT person.Age
FROM [dbo].[Persons] person
WHERE FirstName = '유진'

SELECT * 
FROM [dbo].[Persons]
WHERE Age=3
```

위와 같은 쿼리를 이용하여 특정 필드의 값을 가져올 수도 있고 조건을 설정할 수도 있습니다. 이때 중요한 것은

**String 값을 비교할 때는 무조건 `"` 대신 `'`를 사용해야 한다는 것입니다.** 이것은 SELECT 문 뿐만 아니라 모든 쿼리문에 적용됩니다.

만약 첫번째 쿼리문에서 `WHERE FirstName = '유진'` 대신 `WHERE FirstName = "유진"` 하면 에러가 뜹니다.

2. 데이터 업데이트 UPDATE

```sql
UPDATE [dbo].[UserInfo] 
SET [ConversationCount]=3 
WHERE UserID=1
```
위와 같은 쿼리를 이용하여 데이터베이스에 있는 값을 업데이트 시킬수도 있습니다.

3. 테이블 혹은 데이터베이스 삭제 DROP

```sql
DROP TABLE [dbo].[Persons]

DROP DATABASE healtheeDB
```
위와 같은 쿼리문을 이용하여 데이터베이스나 테이블을 삭제할 수도 있습니다.

4. 데이터 삽입 INSERT

```sql
INSERT INTO [dbo].[Persons]
VALUES(
'유진',
'정',
100
);
```

위와 같이 INSERT INTO VALUES를 이용하여 테이블에 값을 집어넣을 수 있습니다.

### C# 코드 상에서 데이터베이스 이용하기

우선 프로젝트에서 데이터베이스와 연결해줘야 합니다.

```C#
try
{
   SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
   builder.ConnectionString = "연결 스트링";

   using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
   {
      connection.Open();

      //여기에 원하는 쿼리문을 넣어 쿼리를 실행시킵니다.
                    
      connection.Close();
   }
}
catch (SqlException e)
{
}
```

위의 코드를 이용하여 데이터베이스와 연결합니다.

`연결 스트링` 부분에 데이터 베이스와 연결할 수 있는 값을 넣어줘야 연결이 됩니다. 이 값은 

![image](https://user-images.githubusercontent.com/41438361/88478722-8825cc80-cf85-11ea-9426-089bd226437e.png)

Microsoft Azure Portal의 리소스 그룹에서 해당 데이터베이스를 선택한 뒤 설정 > 연결 문자열에 있는 값을 그대로 복사해서 넣어주시면 됩니다. 

그 다음에는 쿼리문을 이용하여 데이터 베이스를 조작합니다.

**SELECT를 이용할 경우와 INSERT, UPDATE를 이용할 때 사용해야 하는 함수가 다릅니다.**

```C#
StringBuilder sb = new StringBuilder();
sb.Append("SELECT COUNT(*) FROM [dbo].[ExerciseRecord] WHERE UserID='" + UserInfoManager.keyNum + "'"); //유저의 고유번호로 DB에서 사용자 찾기

String sql = sb.ToString();

int count = 0;

using (SqlCommand command = new SqlCommand(sql, connection))
{
   using (SqlDataReader reader = command.ExecuteReader())
   {
      while (reader.Read())
      {
         count = reader.GetInt32(0);
      }
   }
}
```

쿼리를 넣어야 하는 부분에 위의 코드를 넣으면 SELECT를 이용할 수 있습니다. 

위의 코드에서는 `reader.GetInt32(0)`를 이용하여 Count의 결과를 가져왔는데, `(int)reader.GetValue(0)`를 이용하여 값을 가져오고 타입을 변환할 수도 있습니다. `0`은 가져올 값이 몇번째 필드에 있는 값인지 그 인덱스를 나타낸 것입니다. 

![image](https://user-images.githubusercontent.com/41438361/88478857-6aa53280-cf86-11ea-8c2b-292cc238ad61.png)

만약 Persons 데이터베이스가 위와 같이 구성되어 있고 이 중 FirstName, Age만 가져오고 싶다면 다음과 같이 받아오면 됩니다.

```C#
firstName = (string)reader.GetValue(0);
age = (int)reader.GetValue(2);
```

그렇다면 UPDATE 와 INSERT는 어떻게 이용해야 하는지 보겠습니다.

```
SqlCommand q;
string query;

query = "INSERT INTO [dbo].[ExerciseRecord] VALUES(" + UserInfoManager.keyNum + ", 0, 0, 0, 0, 0);"; 

q = new SqlCommand(query, connection);
q.ExecuteNonQuery();
```

위와 같이 **`ExecuteNonQuery()`** method를 이용해서 실행시켜야 합니다.

## 그 외 알아낸 소소한 것들

### 모든 Dialog들은 ComponentDialog를 상속해야 합니다.

```C#
public class RecordDialog : ComponentDialog
```

### C# Project(Visual Studio)에서 Azure Sql Database 보기

![image](https://user-images.githubusercontent.com/41438361/88479130-f53a6180-cf87-11ea-9fe7-ce1e9a74e586.png)

Sql 개체 탐색기를 열어줍니다.

![image](https://user-images.githubusercontent.com/41438361/88479163-17cc7a80-cf88-11ea-8d04-ea6175213e22.png)

개체 탐색기에서 SQL Server 추가를 눌러줍니다.

![image](https://user-images.githubusercontent.com/41438361/88479194-4ea29080-cf88-11ea-81d4-0c60b13ff94b.png)

인증을 SQL Server 인증으로 하고, 여기에 서버 이름, 사용자 이름, 암호를 넣어줘야 합니다. 이 값들은 azure portal에서 확인할 수 있습니다.

![18](https://user-images.githubusercontent.com/41438361/88479259-c1ac0700-cf88-11ea-97d7-50e0acff3bd5.JPG)

위 처럼 서버 이름은 SQL 데이터베이스의 개요 탭에서 확인할 수 있습니다. 그외의 값들까지 넣어주고 연결하면

![image](https://user-images.githubusercontent.com/41438361/88479295-ef914b80-cf88-11ea-82d5-2f0a2db23747.png)

위와 같이 데이터베이스가 연결된 것을 확인할 수 있습니다.

### Prompt의 RetryPrompt가 안 보여지는 이유

```C#
var promptOptions = new PromptOptions
{
   Prompt = MessageFactory.Text("목표 체중을 알려주세요(단위 : kg)"),
   RetryPrompt = MessageFactory.Text("0보다 크고 300보다 작은 수치로 적어주세요."),
};
```

위와 같이 코드를 짜면 원래 prompt의 특정 조건을 만족하지 않거나 유효하지 않은 값을 사용자가 입력할 경우 retryprompt를 봇이 다시 전송해야 합니다.

하지만 retryprompt를 봇이 출력하기 전에 봇이 사용자에게 또 메세지를 보내는 작업이 있을 경우 retryprompt가 실행이 되지 않습니다.

이게 무슨 말이냐면,

![KakaoTalk_20200726_215055163](https://user-images.githubusercontent.com/41438361/88479464-477c8200-cf8a-11ea-971c-86a158edb432.jpg)

원래 정상적으로 동작하는 retryprompt 의 경우 위처럼 작동해야 합니다.

하지만

![KakaoTalk_20200726_215108881](https://user-images.githubusercontent.com/41438361/88479473-59f6bb80-cf8a-11ea-9ec2-4739c23fe7db.jpg)

위와 같이 다른 코드에서 봇이 사용자에게 메세지를 보내는 코드가 있을 경우(저같은 경우에는 `OnMessageActivityAsync`에서 사용자의 입력이 들어올때마다 모드 확인을 위해 현재 모드를 봇이 사용자에게 메세지를 보내게 했습니다.) RetryPrompt가 보여지지 않게 됩니다. 이 경우 봇이 에러가 나지는 않지만 원래 의도했던 대로 retryprompt 가 사용자에게 보여지지 않습니다.

### HeroCard의 text는 markdown 문법을 사용한다.

```C#
var heroCard = new HeroCard
{
    Title = "#### 1. 운동 추천",
    Text = "운동 부위, 운동 종류를 설정하여 맞춤 운동을 추천받아보세요! 운동 방법과 운동할 세트, 시간, 자세 등 운동 방법을 영상과 함께 알려드립니다."
};
```

위와 같이 herocard를 만들 경우, `Title`의 "####"은 보여지지 않습니다. markdown 문법에서 제목 형식으로 바꿔주는 문법으로 인식이 되어 보여집니다.

### QnA Maker

![image](https://user-images.githubusercontent.com/41438361/88479711-1dc45a80-cf8c-11ea-8837-486b1b02742d.png)

QnA Maker에 질문-답변 쌍을 추가하면 위처럼 보입니다. 이를 QnA Maker에서 위의 "Test" 눌러 테스트 할 경우, 빈칸 없이 "목적이뭐야"라고 물었을 경우 QnA Maker가 잘 답변하는 것을 확인할 수 있었습니다.

하지만 이를 봇과 연결하여 Emulator로 돌렸을 때, "목적이뭐야"라고 띄어쓰기 없이 입력했을 경우 QnA Maker에서 올바른 답을 찾을 수 없다는 메세지가 떴습니다. 이런 점을 주의해서 질문-답변 쌍을 만드시면 좋을 것 같습니다.

## 더 발전할 수 있는 부분

1. 사용자의 입력을 처리할 때 LUIS(자연어처리를 도와준다)를 이용하여 사용자의 입력을 효과적으로 처리할 수 있게 한다.
2. 사용자가 자신이 포즈를 취하는 사진을 봇에 보냈을 경우 인공지능 모델을 활용하여 기존 자세와 비교하여 어떤 점이 잘못되었는지 알려준다.
3. 사용자가 음식 검색도 할 수 있어 음성 메세지로 음식 이름을 말하면 어떤 음식을 말하는지 확인하여 음식 정보를 보여준다.
