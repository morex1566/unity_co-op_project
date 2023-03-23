using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    // ������ �� ��Ͽ� ���� ������ ���� ����ü.
    public struct CreationInfo
    {
        public Block.TYPE block_type; // ����� ����.
        public int max_count; // ����� �ִ� ����.
        public int height; // ����� ��ġ�� ����.
        public int current_count; // �ۼ��� ����� ����.
    };
    public CreationInfo previous_block; // ������ � ����� ������°�.
    public CreationInfo current_block; // ���� � ����� ������ �ϴ°�.
    public CreationInfo next_block; // ������ � ����� ������ �ϴ°�.
    public int block_count = 0; // ������ ����� �� ��.
    public int level = 0; // ���̵�

    private void clear_next_block(ref CreationInfo block)
    {
        // ���޹��� ���(block)�� �ʱ�ȭ. 
        block.block_type = Block.TYPE.FLOOR;
        block.max_count = 15;
        block.height = 0;
        block.current_count = 0;
    }
    // ������ ��Ʈ�� �ʱ�ȭ�Ѵ�.
    public void initialize()
    {
        this.block_count = 0; // ����� �� ���� �ʱ�ȭ.
                              // ����, ����, ���� ����� ����.
                              // clear_next_block()�� �Ѱܼ� �ʱ�ȭ�Ѵ�.
        this.clear_next_block(ref this.previous_block);
        this.clear_next_block(ref this.current_block);
        this.clear_next_block(ref this.next_block);
    }
    // ref: ������ ���� �μ��� ������ ���� ȣ���ϴ� �ʰ� ȣ��Ǵ� �� ��� �μ��� �ʿ�

    private void update_level(ref CreationInfo current, CreationInfo previous)
    {
        switch (previous.block_type)
        {
            case Block.TYPE.FLOOR: // �̹� ����� �ٴ��� ���.
                current.block_type = Block.TYPE.HOLE; // ���� ���� ������ �����.
                current.max_count = 5; // ������ 5�� �����.
                current.height = previous.height; // ���̸� ������ ���� �Ѵ�.
                break;
            case Block.TYPE.HOLE: // �̹� ����� ������ ���.
                current.block_type = Block.TYPE.FLOOR; // ������ �ٴ� �����.
                current.max_count = 10; // �ٴ��� 10�� �����.
                break;
        }
    }
    public void update()
    { // *Update()�� �ƴ�. create_floor_block() �޼��忡�� ȣ��
        this.current_block.current_count++; // �̹��� ���� ��� ������ ����.
                                            // �̹��� ���� ��� ������ max_count �̻��̸�.
        if(this.current_block.current_count >= this.current_block.max_count) {
            this.previous_block = this.current_block;
            this.current_block = this.next_block;
            this.clear_next_block(ref this.next_block); // ������ ���� ����� ������ �ʱ�ȭ.
            this.update_level(ref this.next_block, this.current_block); // ������ ���� ����� ����.
        }
        this.block_count++; // ����� �� ���� ����.
    }

}
