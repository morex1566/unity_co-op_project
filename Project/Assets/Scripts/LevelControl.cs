using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LevelControl.cs (1/2)
// LevelControl class �տ� �߰�
public class LevelData
{
    public struct Range
    { // ������ ǥ���ϴ� ����ü.
        public int min; // ������ �ּڰ�.
        public int max; // ������ �ִ�.
    };

    public float end_time; // ���� �ð�.
    public float player_speed; // �÷��̾��� �ӵ�.
    public Range floor_count; // ���� ���� ���� ����.
    public Range hole_count; // ������ ���� ����.
    public Range height_diff; // ������ ���� ����.

    public LevelData()
    {
        this.end_time = 15.0f; // ���� �ð� �ʱ�ȭ.
        this.player_speed = 6.0f; // �÷��̾��� �ӵ� �ʱ�ȭ.
        this.floor_count.min = 10; // ���� ���� ���� �ּڰ��� �ʱ�ȭ.
        this.floor_count.max = 10; // ���� ���� ���� �ִ��� �ʱ�ȭ.
        this.hole_count.min = 2; // ���� ������ �ּڰ��� �ʱ�ȭ.
        this.hole_count.max = 6; // ���� ������ �ִ��� �ʱ�ȭ.
        this.height_diff.min = 0; // ���� ���� ��ȭ�� �ּڰ��� �ʱ�ȭ.
        this.height_diff.max = 0; // ���� ���� ��ȭ�� �ִ��� �ʱ�ȭ.
    }
}


public class LevelControl
{
    // ������ �� ���Ͽ� ���� ������ ���� ����ü.
    public struct CreationInfo
    {
        public Block.TYPE block_type; // ������ ����.
        public int max_count; // ������ �ִ� ����.
        public int height; // ������ ��ġ�� ����.
        public int current_count; // �ۼ��� ������ ����.
    };
    public CreationInfo previous_block; // ������ � ������ ������°�.
    public CreationInfo current_block; // ���� � ������ ������ �ϴ°�.
    public CreationInfo next_block; // ������ � ������ ������ �ϴ°�.
    public int block_count = 0; // ������ ������ �� ��.
    public int level = 0; // ���̵�
    public int HEIGHT_MAX = 20;
    public int HEIGHT_MIN = -4;

    private List<LevelData> level_datas = new List<LevelData>();


    private void clear_next_block(ref CreationInfo block)
    {
        // ���޹��� ����(block)�� �ʱ�ȭ. 
        block.block_type = Block.TYPE.FLOOR;
        block.max_count = 15;
        block.height = 0;
        block.current_count = 0;
    }
    // ������ ��Ʈ�� �ʱ�ȭ�Ѵ�.
    public void initialize()
    {
        this.block_count = 0; // ������ �� ���� �ʱ�ȭ.
                              // ����, ����, ���� ������ ����.
                              // clear_next_block()�� �Ѱܼ� �ʱ�ȭ�Ѵ�.
        this.clear_next_block(ref this.previous_block);
        this.clear_next_block(ref this.current_block);
        this.clear_next_block(ref this.next_block);
    }
    // ref: ������ ���� �μ��� ������ ���� ȣ���ϴ� �ʰ� ȣ��Ǵ� �� ��� �μ��� �ʿ�

    private void update_level(ref CreationInfo current, CreationInfo previous, float passage_time)
    { // �� �μ� passage_time���� �÷��� ��� �ð��� �޴´�.
      // ���� 1~���� 5�� �ݺ��Ѵ�.
        float local_time = Mathf.Repeat(passage_time, this.level_datas[this.level_datas.Count - 1].end_time);
        // ���� ������ ���Ѵ�.
        int i;
        for (i = 0; i < this.level_datas.Count - 1; i++)
        {
            if (local_time <= this.level_datas[i].end_time)
            {
                break;
            }
        }
        this.level = i;
        current.block_type = Block.TYPE.FLOOR;
        current.max_count = 1;
        if (this.block_count >= 10)
        {
            // ���� ������ ���� �����͸� �����´�.
            LevelData level_data;
            level_data = this.level_datas[this.level];
            switch (previous.block_type)
            {
                case Block.TYPE.FLOOR: // ���� ������ �ٴ��� ���.
                    current.block_type = Block.TYPE.HOLE; // �̹��� ������ �����.
                                                          // ���� ũ���� �ּڰ�~�ִ� ������ ������ ��.
                    current.max_count = Random.Range(level_data.hole_count.min, level_data.hole_count.max);
                    current.height = previous.height; // ���̸� ������ ���� �Ѵ�.
                    break;
                case Block.TYPE.HOLE: // ���� ������ ������ ���.
                    current.block_type = Block.TYPE.FLOOR; // �̹��� �ٴ��� �����.
                                                           // �ٴ� ������ �ּڰ�~�ִ� ������ ������ ��.
                    current.max_count = Random.Range(level_data.floor_count.min, level_data.floor_count.max);
                    // �ٴ� ������ �ּڰ��� �ִ��� ���Ѵ�.
                    int height_min = previous.height + level_data.height_diff.min;
                    int height_max = previous.height + level_data.height_diff.max;
                    height_min = Mathf.Clamp(height_min, HEIGHT_MIN, HEIGHT_MAX); // �ּҿ� �ִ밪 ���̸� ������ ����
                    height_max = Mathf.Clamp(height_max, HEIGHT_MIN, HEIGHT_MAX);
                    // �ٴ� ������ �ּڰ�~�ִ� ������ ������ ��.
                    current.height = Random.Range(height_min, height_max);
                    break;
            }
        }
    }


    public void update(float passage_time)
    { // *Update()�� �ƴ�. create_floor_block() �޼��忡�� ȣ��
        this.current_block.current_count++; // �̹��� ���� ���� ������ ����.
                                            // �̹��� ���� ���� ������ max_count �̻��̸�.
        if (this.current_block.current_count >= this.current_block.max_count) {
            this.previous_block = this.current_block;
            this.current_block = this.next_block;
            this.clear_next_block(ref this.next_block); // ������ ���� ������ ������ �ʱ�ȭ.

            // this.update_level(ref this.next_block, this.current_block);
            this.update_level(ref this.next_block, this.current_block, passage_time);
        }
        this.block_count++; // ������ �� ���� ����.
    }


    public void loadLevelData(TextAsset level_data_text)
    {
        string level_texts = level_data_text.text; // �ؽ�Ʈ �����͸� ���ڿ��� �����´�.
        string[] lines = level_texts.Split('\n'); // ���� �ڵ� '\'���� �����ؼ� ���ڿ� �迭�� �ִ´�.

        foreach (var line in lines)
        {
            if (line == "")
            { // ���� �� ���̸�.
                continue; // �Ʒ� ó���� ���� �ʰ� �ݺ����� ó������ �����Ѵ�.
            };
            Debug.Log(line); // ���� ������ ����� ����Ѵ�.
            string[] words = line.Split(); // �� ���� ���带 �迭�� �����Ѵ�.
            int n = 0;
            // LevelData�� ������ �����Ѵ�.
            // ���� ó���ϴ� ���� �����͸� �־� ����.
            LevelData level_data = new LevelData();
            // words���� �� ���忡 ���ؼ� ������� ó���� ���� ����.

            foreach (var word in words)
            {
                if(word.StartsWith("#")) 
                { // ������ ���۹��ڰ� #�̸�.
                    break;
                } // ���� Ż��.

                if (word == "")
                { // ���尡 �� �������.
                    continue;
                } // ������ �������� �����Ѵ�.

                // n ���� 0, 1, 2,...7�� ��ȭ���� �����ν� 8�׸��� ó���Ѵ�.
                // �� ���带 �÷԰����� ��ȯ�ϰ� level_data�� �����Ѵ�.
                switch (n)
                {
                    case 0: level_data.end_time = float.Parse(word); break;
                    case 1: level_data.player_speed = float.Parse(word); break;
                    case 2: level_data.floor_count.min = int.Parse(word); break;
                    case 3: level_data.floor_count.max = int.Parse(word); break;
                    case 4: level_data.hole_count.min = int.Parse(word); break;
                    case 5: level_data.hole_count.max = int.Parse(word); break;
                    case 6: level_data.height_diff.min = int.Parse(word); break;
                    case 7: level_data.height_diff.max = int.Parse(word); break;
                }
                n++;
            }

            if (n >= 8)
            { // 8�׸�(�̻�)�� ����� ó���Ǿ��ٸ�.
                this.level_datas.Add(level_data); // List ������ level_datas�� level_data�� �߰��Ѵ�.
            }
            else
            { // �׷��� �ʴٸ�(������ ���ɼ��� �ִ�).
                if (n == 0)
                { // 1���嵵 ó������ ���� ���� �ּ��̹Ƿ�.
                  // ��������. �ƹ��͵� ���� �ʴ´�.
                }
                else
                { // �� �̿��̸� ������.
                  // ������ ������ ���� �ʴٴ� ���� �����ִ� ���� �޽����� ǥ���Ѵ�.
                    Debug.LogError("[LevelData] Out of parameter.\n");
                }
            }
        }

        if (this.level_datas.Count == 0)
        {   // level_datas�� �����Ͱ� �ϳ��� ������.
            Debug.LogError("[LevelData] Has no data.\n"); // ���� �޽����� ǥ���Ѵ�.
            this.level_datas.Add(new LevelData()); // level_datas�� �⺻ LevelData�� �ϳ� �߰��� �д�.
        }
    }

    public float getPlayerSpeed()
    {
        return (this.level_datas[this.level].player_speed);
    }
}
