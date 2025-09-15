using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LearnToObserver
{
    /*
     * 다형성은 부모의 포인터에 자식의 객체를 동적할당하는 것
     * 다형성을 이용할때 자식의 함수에 접근하려면 virtual키워드를 사용
     * 동적바인딩은 함수의 역할이 런타임 중에 결정되는 것을 말한다.
     * 부모가 같은 객체를 한꺼번에 관리하려면 다형성을 이용한다.
     * 객체를 한꺼번에 관리하려면 STL을 사용한다.
     */
    class Program
    { 
        static void Main()
        {

            //Commander.Move(10,10) //10 10 만큼 이동
            //unitCollection.Move(x,y) //선택한녀석 단체이동
        }

        
    }

    public class unitCollection
    {
        
    }

    public class Commander : Unit
    {
        public void SetUnit()
        {
            Marin marin = new Marin();
            List<Unit> unitCollections = new List<Unit>();
        }
        // 유닛 리스트에 자동 저장
        

        // 유닛 선택후 선택한 유닛 리스트에 저장
        public void SelectUnit()
        {

        }

        // 유닛 선택후 이미선택되었는지 확인
        // 이미 선택되었다면 선택한 유닛 리스트에서 제거
        public void DeselectUnit()
        {

        }

        // 선택한 유닛 리스트의 유닛에게 무브 오더
        public void OderMove()
        {
            // 유닛 선택
            // 유닛 이동
        }
    }

    public abstract class Unit
    {
        protected string name;
        protected int positionX;
        protected int positionY;

        // 이동 명령
        public virtual void Move(int x, int y)
        {
            this.positionX += x;
            this.positionY += y;
            Console.WriteLine(this.name + ": (" + this.positionX + "," + this.positionY + ")위치로 이동 완료");
        }
    }

    public class Marin : Unit
    {
        public Marin()
        {
            this.name = "마린";
            this.positionX = 0;
            this.positionY = 0;
        }

        public override void Move(int x, int y)
        {
            this.positionX += x;
            this.positionY += y;
            Console.WriteLine(this.name+": ("+this.positionX +","+this.positionY +")위치로 이동 완료");
        }
    }



}
